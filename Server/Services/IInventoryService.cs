using Server.Entities;
using Server.Repositories;

namespace Server.Services;

public interface IInventoryService
{
    Task StockIn(int productId, int qty, int userId);
    Task StockOut(int productId, int qty, int userId);
    Task<InventoryMaster?> GetInventoryByProductId(int productId);
}

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repo;

    public InventoryService(IInventoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<InventoryMaster?> GetInventoryByProductId(int productId)
    {
        if (productId <= 0)
            throw new Exception("Invalid product id");

        return await _repo.GetInventoryByProductId(productId);
    }

    public async Task StockIn(int productId, int qty, int userId)
    {
        if (productId <= 0)
            throw new Exception("Invalid product id");

        if (qty <= 0)
            throw new Exception("Quantity must be greater than 0");

        var inventory = await _repo.GetInventoryByProductId(productId);

        if (inventory == null)
            throw new Exception("Inventory not found");

        inventory.quantity += qty;
        inventory.modify_date = DateTime.UtcNow;
        inventory.modify_by = userId;

        await _repo.UpdateInventory(inventory);
    }

    public async Task StockOut(int productId, int qty, int userId)
    {
        if (productId <= 0)
            throw new Exception("Invalid product id");

        if (qty <= 0)
            throw new Exception("Quantity must be greater than 0");

        var inventory = await _repo.GetInventoryByProductId(productId);

        if (inventory == null)
            throw new Exception("Inventory not found");

        if (inventory.quantity < qty)
            throw new Exception("Insufficient stock");

        inventory.quantity -= qty;
        inventory.modify_date = DateTime.UtcNow;
        inventory.modify_by = userId;

        await _repo.UpdateInventory(inventory);
    }
}