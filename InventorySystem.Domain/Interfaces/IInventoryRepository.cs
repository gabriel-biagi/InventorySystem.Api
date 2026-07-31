using InventorySystem.Domain.Entities;
namespace InventorySystem.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        InventoryItem? GetByProductId(int productId);
        void Add(InventoryItem inventoryItem);
        void Update(InventoryItem inventoryItem);
        List<InventoryItem> GetAll();
    }
}
