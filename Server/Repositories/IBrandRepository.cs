using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Persistence;
using System.Linq.Expressions;
namespace Server.Repositories;

public interface IBrandRepository
{
    Task<IEnumerable<BrandMaster>> FindAsync(Expression<Func<BrandMaster, bool>> predicate);
    Task AddAsync(BrandMaster brand);
    Task UpdateAsync(BrandMaster brand);
    Task DeleteAsync(BrandMaster brand);
}
public class BrandRepository : IBrandRepository
{
    private readonly AppDbContext _context;

    public BrandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BrandMaster>> FindAsync(Expression<Func<BrandMaster, bool>> predicate)
    {
        return await _context.DbBrandMaster
            .Where(x => x.is_active)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(BrandMaster brand)
    {
        await _context.DbBrandMaster.AddAsync(brand);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BrandMaster brand)
    {
        _context.DbBrandMaster.Update(brand);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(BrandMaster brand)
    {
        brand.is_active = false;
        _context.DbBrandMaster.Update(brand);
        await _context.SaveChangesAsync();
    }
}
