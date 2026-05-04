using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Persistence;

namespace Server.Repositories;

public interface IInventoryRepository
{
    Task<InventoryMaster?> GetInventoryByProductId(int productId);
    Task<InventoryMaster> CreateInventory(InventoryMaster entity);
    Task UpdateInventory(InventoryMaster entity);
}
public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _context;
    public InventoryRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<InventoryMaster?> GetInventoryByProductId(int productId) { 
        return await  _context.DbInventoryMaster
            .FirstOrDefaultAsync(x => x.product_id == productId);
    }

    public async Task<InventoryMaster> CreateInventory(InventoryMaster entity)
    {
        await _context.DbInventoryMaster.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task UpdateInventory(InventoryMaster entity)
    {
        var existing = await _context.DbInventoryMaster
            .FirstOrDefaultAsync(x => x.inventory_id == entity.inventory_id);

        if (existing == null)
            throw new Exception("Inventory not found");

        existing.quantity = entity.quantity;
        existing.modify_date = DateTime.UtcNow;
        existing.modify_by = entity.modify_by;

        await _context.SaveChangesAsync();
    }
}
