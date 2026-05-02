using Server.Entities;
using Server.Repositories;
using System.Linq.Expressions;
namespace Server.Services;

public interface IProductImageService
{
    Task<IEnumerable<ProductImageTran>> FindAsync(Expression<Func<ProductImageTran, bool>> predicate);
    Task SetDefault(int image_id);
    Task DeleteAllAsync(int productId);
    Task DeleteAsync(int imageId);
}

public class ProductImageService : IProductImageService
{
    private readonly IProductImageRepository _repo;
    public ProductImageService(IProductImageRepository repo)
    {
        _repo = repo;
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