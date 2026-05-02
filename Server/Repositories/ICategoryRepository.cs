using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Persistence;
using System.Linq.Expressions;

namespace Server.Repositories;

public interface ICategoryRepository
{
    Task<List<CategoryMaster>> FindAsync(Expression<Func<CategoryMaster, bool>> predicate);
    Task AddAsync(CategoryMaster category);
    Task UpdateAsync(CategoryMaster category);
    Task DeleteAsync(CategoryMaster category);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryMaster>> FindAsync(Expression<Func<CategoryMaster, bool>> predicate)
    {
        return await _context.DbCategorieMaster
            .Where(predicate).ToListAsync();

    }

    public async Task AddAsync(CategoryMaster category)
    {
        await _context.DbCategorieMaster.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryMaster category)
    {
        _context.DbCategorieMaster.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CategoryMaster category)
    {
        category.is_active = false;
        _context.DbCategorieMaster.Update(category);
        await _context.SaveChangesAsync();
    }
}