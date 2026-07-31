using InventorySystem.Domain.Entities;

namespace InventorySystem.Domain.Interfaces
{
    public interface IProductRepository
    {
        Product? GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
