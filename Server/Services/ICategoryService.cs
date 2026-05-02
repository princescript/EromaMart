using Server.Entities;
using Server.Repositories;
using System.Linq.Expressions;

namespace Server.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryMaster>> FindAsync(Expression<Func<CategoryMaster, bool>> predicate);
    Task AddAsync(CategoryMaster category);
    Task UpdateAsync(int id, CategoryMaster category);
    Task DeleteAsync(int id);
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<CategoryMaster>> FindAsync(Expression<Func<CategoryMaster, bool>> predicate)
    {
        return await _repo.FindAsync(predicate);
    }

    public async Task AddAsync(CategoryMaster category)
    {
        if (string.IsNullOrWhiteSpace(category.category_name))
            throw new Exception("Category name is required");

        category.is_active = true;
        category.create_date = DateTime.Now;
        category.create_by = 1;

        var exists = (await _repo.FindAsync(x => x.category_name == category.category_name)).Any();
        if (exists)
            throw new Exception("Category already exists");

        await _repo.AddAsync(category);
    }

    public async Task UpdateAsync(int id, CategoryMaster category)
    {
        var result = (await _repo.FindAsync(x => x.category_id == id)).FirstOrDefault();

        if (result == null)
            throw new Exception("Category not found");

        if (string.IsNullOrWhiteSpace(category.category_name))
            throw new Exception("Category name is required");

        result.category_name = category.category_name;
        result.modify_date = DateTime.Now;
        result.modify_by = 1;

        await _repo.UpdateAsync(result);
    }

    public async Task DeleteAsync(int id)
    {
        var category = (await _repo.FindAsync(x => x.category_id == id)).FirstOrDefault();

        if (category == null)
            throw new Exception("Category not found");

        category.is_active = false;
        category.modify_date = DateTime.Now;
        category.modify_by = 1;

        await _repo.UpdateAsync(category);
    }
}