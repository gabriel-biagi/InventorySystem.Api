using AutoMapper;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Application.Services.Interfaces;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Exception;
using InventorySystem.Domain.Interfaces;

namespace InventorySystem.Application.Services;

public class InventoryItemService : IInventoryItemService
{
    private  readonly IInventoryRepository _repository;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;

    public InventoryItemService(IInventoryRepository repository, IMapper mapper,  IProductRepository productRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _productRepo = productRepository;
    }
    
    public async Task<IEnumerable<InventoryItemResponse>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        var itemsDto = _mapper.Map<IEnumerable<InventoryItemResponse>>(items);
        return itemsDto;
    }

    public async Task<InventoryItemResponse> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Inventory code must be greater than 0");
        }
        
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            throw new NotFoundException("No items found");
        }
        var itemDto = _mapper.Map<InventoryItemResponse>(item);
        return itemDto;
    }

    public async Task<IEnumerable<InventoryItemResponse>> GetItemsByProductIdAsync(int productId)
    {
        if (productId <= 0)
        {
            throw new ArgumentException("Product code must be greater than 0");
        }
        var items =  await _repository.GetItemsByProductIdAsync(productId);
        
        var itemsDto = _mapper.Map<IEnumerable<InventoryItemResponse>>(items);
        return itemsDto;
    }

    public async Task<InventoryItemResponse> AddAsync(int productId, InventoryItemRequest request)
    {
        if (productId <= 0)
        {
            throw new ArgumentException("Product code must be greater than 0");
        }
        
        var product = await _productRepo.GetByIdAsync(productId);
        if (product is null)
        {
            throw new NotFoundException("No products found");
        }
        
        var existingItems = await _repository.GetItemsByProductIdAsync(productId);
        if (existingItems.Any(i => 
                i.Location.Column == request.Column && 
                i.Location.Shelf == request.Shelf && 
                i.Location.Item == request.Item))
        {
            throw new BusinessException("Location already occupied on this product");
        }
        
        
        var location = new Location(request.Column, request.Shelf, request.Item);
        var inventoryItem = new InventoryItem(product, location, request.Quantity);
        
        await _repository.AddAsync(inventoryItem);
        var inventoryItemDto = _mapper.Map<InventoryItemResponse>(inventoryItem);
        return inventoryItemDto;
    }

    public async Task<InventoryItemResponse> UpdateAsync(int id, decimal quantity)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Inventory code must be greater than 0");
        }
        
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            throw new NotFoundException("No items found");
        }
        
        item.AddQuantity(quantity);
        await _repository.UpdateAsync(item);
        
        var itemDto = _mapper.Map<InventoryItemResponse>(item);
        return itemDto;
    }
    
    public async Task<InventoryItemResponse> RemoveAsync(int id, decimal quantity)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Inventory code must be greater than 0");
        }
        
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            throw new NotFoundException("No items found");
        }
        
        item.RemoveQuantity(quantity);
        await _repository.UpdateAsync(item);
        
        var itemDto = _mapper.Map<InventoryItemResponse>(item);
        return itemDto;
    }
    

    public async Task DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Inventory code must be greater than 0");
        }
        
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            throw new NotFoundException("No items found");
        }
        await _repository.DeleteAsync(item);
    }
}