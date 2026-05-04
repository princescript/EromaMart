using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Persistence;
using System.Linq.Expressions;
namespace Server.Repositories;

public interface IProductImageRepository
{
    Task<bool> AddAsync(List<ProductImageTran> list);

    Task<List<ProductImageTran>> FindAsync(Expression<Func<ProductImageTran, bool>> predicate);
    Task<ProductImageTran?> GetByIdAsync(int imageId);
    Task DeleteAllAsync(List<ProductImageTran> images);
    Task DeleteAsync(ProductImageTran image);
    Task SaveChangesAsync();
}
public class ProductImageRepository : IProductImageRepository
{
    private readonly AppDbContext _context;

    public ProductImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(List<ProductImageTran> list)
    {
        await _context.DbProductImageTran.AddRangeAsync(list);

        var result = await _context.SaveChangesAsync();

        return result > 0;
    }
    public async Task<List<ProductImageTran>> FindAsync(Expression<Func<ProductImageTran, bool>> predicate)
    {
        return await _context.DbProductImageTran
            .Where(x => x.is_active)
            .Where(predicate)
            .ToListAsync();
    }
    public async Task<ProductImageTran?> GetByIdAsync(int imageId)
    {
        return await _context.DbProductImageTran
            .FirstOrDefaultAsync(x => x.image_id == imageId && x.is_active);
    }
    public async Task DeleteAllAsync(List<ProductImageTran> images)
    {
        var now = DateTime.UtcNow;

        foreach(var x in images)
        {
            x.is_active = false;
            x.modify_date = now;
            x.modify_by = 1;
        };
        await SaveChangesAsync();
    }
    public async Task DeleteAsync(ProductImageTran image)
    {
        image.is_active = false;
        image.modify_date = DateTime.UtcNow;
        image.modify_by = 1;
        await SaveChangesAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}