using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Pagination;

namespace InventorySystem.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        Task<PagedList<InventoryItem>> GetAllAsync(InventoryItemsParameters inventoryItemsParameters);
        Task<InventoryItem?> GetByIdAsync(int id);
        Task<IEnumerable<InventoryItem>> GetItemsByProductIdAsync(int productId);
        Task<Product?> GetProductByIdAsync(int productId);
        Task <InventoryItem> AddAsync(InventoryItem inventoryItem);
        Task<InventoryItem> UpdateAsync(InventoryItem inventoryItem);
        Task DeleteAsync(InventoryItem inventoryItem);
    }
}
