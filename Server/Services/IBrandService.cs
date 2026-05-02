using Server.Entities;
using Server.Repositories;
using System.Linq.Expressions;
namespace Server.Services;

public interface IBrandService
{
    Task<IEnumerable<BrandMaster>> FindAsync(Expression<Func<BrandMaster, bool>> predicate);
    Task AddAsync(BrandMaster brand);
    Task UpdateAsync(int id, BrandMaster brand);
    Task DeleteAsync(int id);
}

public class BrandService : IBrandService
{
    private readonly IBrandRepository _repo;

    public BrandService(IBrandRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<BrandMaster>> FindAsync(Expression<Func<BrandMaster, bool>> predicate)
    {
        return await _repo.FindAsync(predicate);
    }

    public async Task AddAsync(BrandMaster brand)
    {
        if (string.IsNullOrWhiteSpace(brand.brand_name))
            throw new Exception("Brand name is required");

        brand.create_date = DateTime.UtcNow;
        brand.is_active = true;
        brand.is_verified = false;
        await _repo.AddAsync(brand);
    }

    public async Task UpdateAsync(int id, BrandMaster entity)
    {
        var brands = await _repo.FindAsync(x => x.brand_id == id);
        var brand = brands.FirstOrDefault();

        if (brand == null)
            throw new Exception("Brand not found");

        if (string.IsNullOrWhiteSpace(entity.brand_name))
            throw new Exception("Brand name is required");

        brand.brand_name = entity.brand_name;
        brand.description = entity.description;
        brand.gst_number = entity.gst_number;
        brand.pan_number = entity.pan_number;
        brand.website_url = entity.website_url;
        brand.support_email = entity.support_email;
        brand.support_phone = entity.support_phone;
        brand.headquarters_address = entity.headquarters_address;
        brand.country = entity.country;
        brand.state = entity.state;
        brand.logo_url = entity.logo_url;
        brand.is_active = entity.is_active;
        brand.is_verified = entity.is_verified;
        brand.modify_date = DateTime.UtcNow;
        await _repo.UpdateAsync(brand);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _repo.FindAsync(x => x.brand_id == id);
        var brand = existing.FirstOrDefault();

        if (brand == null)
            throw new Exception("Brand not found");

        await _repo.DeleteAsync(brand);
    }
}