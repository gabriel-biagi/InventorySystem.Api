using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;

namespace InventorySystem.Application.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAllAsync();
    Task<ProductResponse> GetByIdAsync(int id);
    Task<ProductResponse> CreateAsync(ProductRequest product);
    Task<ProductResponse> UpdateAsync(int id, string name);
    Task DeleteAsync(int id);
}