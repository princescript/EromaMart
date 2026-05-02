using Server.Entities;
using Server.Repositories;
using System.Linq.Expressions;

public interface IProductService
{
    Task<IEnumerable<ProductMaster>> FindAsync(Expression<Func<ProductMaster, bool>> predicate);
    Task AddAsync(ProductMaster product);
    Task UpdateAsync(string sku, ProductMaster product);
    Task DeleteAsync(string sku);
}
public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ProductMaster>> FindAsync(Expression<Func<ProductMaster, bool>> predicate)
    {
        return await _repo.FindAsync(predicate);
    }


    public async Task AddAsync(ProductMaster product)
    {

        if (string.IsNullOrWhiteSpace(product.product_name))
            throw new Exception("Product name is required");

        if (product.price <= 0)
            throw new Exception("Price must be greater than zero");

        product.is_active = true;
        product.create_date = DateTime.Now;
        product.create_by = 1;
        product.sku = GenerateSku(product.product_name);

        var exists = (await _repo.FindAsync(x => x.sku == product.sku)).Any();
        if (exists)
            throw new Exception("SKU already exists");

        await _repo.AddAsync(product);
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