using Server.DTOs.Image;
using Server.Entities;
using Server.Persistence;
using Server.Repositories;
using Server.Services;
using System.Linq.Expressions;

public interface IProductService
{
    Task<IEnumerable<ProductMaster>> FindPagedAsync(
      Expression<Func<ProductMaster, bool>> predicate,
      int page,
      int pageSize);
    Task<IEnumerable<ProductMaster>> FindAsync(Expression<Func<ProductMaster, bool>> predicate);
    Task<long?> AddAsync(ProductCreateRequest request);
    Task UpdateAsync(string sku, ProductMaster product);
    Task DeleteAsync(string sku);
}
public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IInventoryRepository _inventoryRepo;
    private readonly AppDbContext _context;
    private readonly ICloudinaryService _cloudinary;
    private readonly IProductImageRepository _imageRepo;
    public ProductService(IProductRepository repo,IInventoryRepository inventoryRepo,AppDbContext context, ICloudinaryService service, IProductImageRepository imageRepo)
    {
        _repo = repo;
        _inventoryRepo = inventoryRepo;
        _context = context;
        _cloudinary = service;
        _imageRepo = imageRepo;
    }

    public async Task<IEnumerable<ProductMaster>> FindPagedAsync(
     Expression<Func<ProductMaster, bool>> predicate,
     int page,
     int pageSize)
    {
        return await _repo.FindPagedAsync(
            predicate,
            page,
            pageSize);
    }
    public async Task<IEnumerable<ProductMaster>> FindAsync(Expression<Func<ProductMaster, bool>> predicate)
    {
        return await _repo.FindAsync(predicate);
    }

    public async Task<long?> AddAsync(ProductCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.product_name))
            throw new Exception("Product name is required");

        if (request.price <= 0)
            throw new Exception("Price must be greater than zero");

        var sku = GenerateSku(request.product_name);

        var exists = (await _repo.FindAsync(x => x.sku == sku)).Any();
        if (exists)
            throw new Exception("SKU already exists");

        // 1. Upload images first
        var uploadedImages = await _cloudinary.UploadMultipleAsync(request.Files);

        if (uploadedImages == null || uploadedImages.Count == 0)
            throw new Exception("Image upload failed");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 2. Create product
            var product = await _repo.AddAsync(new ProductMaster
            {
                product_name = request.product_name,
                product_description = request.product_description,
                sku = sku,
                price = request.price,
                discount_percent = request.discount_percent,
                category_id = request.category_id,
                brand_id = request.brand_id,
                is_active = true,
                create_date = DateTime.UtcNow,
                create_by = 1
            });

            // 3. Create inventory
            await _inventoryRepo.CreateInventory(new InventoryMaster
            {
                product_id = product.product_id,
                quantity = 0,
                warehouse_id = 1,
                is_active = true,
                create_date = DateTime.UtcNow,
                create_by = 1
            });

            // 4. Save images
            var imageEntities = uploadedImages.Select((x, index) => new ProductImageTran
            {
                product_id = product.product_id,
                image_url = x.Url,
                public_id = x.PublicId,
                is_default  = false,        
                display_order = index,      
                is_active = true,
                create_date = DateTime.UtcNow,
                create_by = 1
            }).ToList();

            await _imageRepo.AddAsync(imageEntities);

            await transaction.CommitAsync();

            return product.product_id;
        }
        catch
        {
            await transaction.RollbackAsync();

            foreach (var img in uploadedImages)
            {
                await _cloudinary.DeleteAsync(img.PublicId);
            }

            throw;
        }
    }
    public async Task UpdateAsync(string sku, ProductMaster updatedProduct)
    {
        var existing = (await _repo.FindAsync(x => x.sku == sku)).FirstOrDefault();

        if (existing == null)
            throw new Exception("Product not found");

        if (updatedProduct.price <= 0)
            throw new Exception("Price must be greater than zero");

        existing.product_name = updatedProduct.product_name;
        existing.product_description = updatedProduct.product_description;
        existing.brand_id = updatedProduct.brand_id;
        existing.category_id = updatedProduct.category_id;
        existing.price = updatedProduct.price;
        existing.sku = updatedProduct.sku;
        await _repo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(string sku)
    {
        var product = (await _repo.FindAsync(x=>x.sku == sku)).FirstOrDefault();

        if (product == null)
            throw new Exception("Product not found");

        await _repo.DeleteAsync(product);
    }

    private string GenerateSku(string name)
    {
        var prefix = name.Length >= 3 ? name[..3] : name;
        return prefix.ToUpper() + "-" + Guid.NewGuid().ToString("N")[..6];
    }
}