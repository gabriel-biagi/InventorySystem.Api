using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Services.Interfaces;

public interface IInventoryItemService
{
    Task<IEnumerable<InventoryItemResponse>> GetAllAsync();
    Task<InventoryItemResponse> GetByIdAsync(int id);
    Task<IEnumerable<InventoryItemResponse>> GetItemsByProductIdAsync(int productId);
    Task <InventoryItemResponse> AddAsync(int productId, InventoryItemRequest request);
    Task<InventoryItemResponse> UpdateAsync(int id, decimal quantity);
    Task<InventoryItemResponse> RemoveAsync(int id, decimal quantity);
    Task DeleteAsync(int id);
}