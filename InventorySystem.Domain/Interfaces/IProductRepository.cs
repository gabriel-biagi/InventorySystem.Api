using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Pagination;

namespace InventorySystem.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetAllAsync(ProductParameters productParameters);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
