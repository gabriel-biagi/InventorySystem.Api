namespace InventorySystem.Domain
{
    public interface IInventoryRepository
    {
        InventoryItem? GetById(int productId);
        void Add(InventoryItem inventoryItem);
        void Update(InventoryItem inventoryItem);
        List<InventoryItem> GetAll();
    }
}
