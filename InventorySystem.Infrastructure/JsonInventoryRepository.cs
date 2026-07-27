using InventorySystem.Domain;

namespace InventorySystem.Infrastructure
{
    public class JsonInventoryRepository : IInventoryRepository
    {
        public InventoryItem? GetByProductId(int productId)
        {
            throw new NotImplementedException();
        }
        public void Add(InventoryItem inventoryitem)
        {
            throw new NotImplementedException();
        }
        public void Update(InventoryItem inventoryItem)
        {
            throw new NotImplementedException();
        }

        public List<InventoryItem> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
