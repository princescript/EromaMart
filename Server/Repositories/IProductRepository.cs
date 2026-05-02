using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Persistence;
using System.Linq.Expressions;

namespace Server.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductMaster>> FindAsync(Expression<Func<ProductMaster, bool>> predicate);
    Task AddAsync(ProductMaster product);
    Task UpdateAsync(ProductMaster product);
    Task DeleteAsync(ProductMaster product);
}

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductMaster>> FindAsync(Expression<Func<ProductMaster, bool>> predicate)
    {
        return await _context.DbProductMaster
            .Where(x => x.is_active)     
            .Where(predicate)
            .AsNoTracking()              
            .ToListAsync();
    }

    public async Task AddAsync(ProductMaster product)
    {
        await _context.DbProductMaster.AddAsync(product);
        await _context.SaveChangesAsync();

    }

    public async Task UpdateAsync(ProductMaster product)
    {
        _context.DbProductMaster.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductMaster product)
    {
        product.is_active = false;
        _context.DbProductMaster.Update(product);
        await _context.SaveChangesAsync();

    }
}