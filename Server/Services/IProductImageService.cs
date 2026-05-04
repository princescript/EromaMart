using Server.DTOs.Image;
using Server.Entities;
using Server.Repositories;
using System.Linq.Expressions;
namespace Server.Services;

public interface IProductImageService
{
    Task<UploadResult> UploadImageAsync(UploadRequest entity);
    Task<IEnumerable<ProductImageTran>> FindAsync(Expression<Func<ProductImageTran, bool>> predicate);
    Task SetDefault(int image_id);
    Task DeleteAllAsync(int productId);
    Task DeleteAsync(int imageId);
}

public class ProductImageService : IProductImageService
{
    private readonly IProductImageRepository _repo;
    private readonly ICloudinaryService _cloudinary;

    public ProductImageService(IProductImageRepository repo, ICloudinaryService cloudinary)
    {
        _repo = repo;
        _cloudinary = cloudinary;

    }
    public async Task<UploadResult> UploadImageAsync(UploadRequest entity)
    {
        var uploaded = await _cloudinary.UploadMultipleAsync(entity.Files);

        if (uploaded == null || uploaded.Count == 0)
            throw new Exception("Image not uploaded");

        var newImageTran = uploaded.Select(x => new ProductImageTran
        {
            product_id = entity.ProductId,
            image_url = x.Url,
            public_id = x.PublicId,
            is_default = false,
            display_order = 1,
            is_active = true,
            create_date = DateTime.UtcNow,
            create_by = 1
        }).ToList();

        var result = await _repo.AddAsync(newImageTran);

        if (!result)
        {
            foreach (var img in uploaded)
            {
                await _cloudinary.DeleteAsync(img.PublicId);
            }

            return new UploadResult
            {
                Success = false,
                Message = "DB insert failed, cloud images rolled back"
            };
        }

        return new UploadResult
        {
            Success = true,
            Message = "Images Upload successful"
        };
    }
    public async Task<IEnumerable<ProductImageTran>> FindAsync(Expression<Func<ProductImageTran, bool>> predicate)
    {
        return await _repo.FindAsync(predicate);
    }
    public async Task SetDefault(int image_id)
    {
        var image = await _repo.GetByIdAsync(image_id);

        if (image == null)
            throw new Exception("Image not found");

        var productImages = await _repo.FindAsync(x =>
            x.product_id == image.product_id && x.is_active);

        foreach (var img in productImages)
        {
            img.is_default = false;
        }

        image.is_default = true;
        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAllAsync(int productId)
    {
        var images = await _repo.FindAsync(x => x.product_id == productId);
        if (images == null)
            throw new Exception("Product images not found");

        await _repo.DeleteAllAsync(images);

    }
    public async Task DeleteAsync(int imageId)
    {
        var image = await _repo.GetByIdAsync(imageId);

        if (image == null)
            throw new Exception("Product image not found");

        await _repo.DeleteAsync(image);

    }

}