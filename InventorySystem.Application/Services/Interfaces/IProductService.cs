using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Domain.Pagination;

namespace InventorySystem.Application.Services.Interfaces;

public interface IProductService
{
    Task<PagedProductResponse> GetAllAsync(ProductParameters productParameters);
    Task<ProductResponse> GetByIdAsync(int id);
    Task<ProductResponse> CreateAsync(ProductRequest product);
    Task<ProductResponse> UpdateAsync(int id, string name);
    Task DeleteAsync(int id);
}