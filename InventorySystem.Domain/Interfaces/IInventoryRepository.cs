using InventorySystem.Domain.Entities;
namespace InventorySystem.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryItem>> GetAllAsync();
        Task<InventoryItem?> GetByIdAsync(int id);
        Task<IEnumerable<InventoryItem>> GetItemsByProductIdAsync(int productId);
        Task<Product?> GetProductByIdAsync(int productId);
        Task <InventoryItem> AddAsync(InventoryItem inventoryItem);
        Task<InventoryItem> UpdateAsync(InventoryItem inventoryItem);
        Task DeleteAsync(InventoryItem inventoryItem);
        Task DeleteByIdAsync(int id);
    }
}
