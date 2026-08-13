using InventorySystem.Domain.Entities;
namespace InventorySystem.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryItem>> GetAllAsync();
        Task<InventoryItem?> GetByProductIdAsync(int productId);
        Task <InventoryItem> AddAsync(InventoryItem inventoryItem);
        Task<InventoryItem> UpdateAsync(InventoryItem inventoryItem);
        Task DeleteAsync(InventoryItem inventoryItem);
    }
}
