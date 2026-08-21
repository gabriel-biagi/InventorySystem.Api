using AutoMapper;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Application.Services.Interfaces;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Exception;
using InventorySystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Application.Services;

public class ProductService : IProductService
{
    private  readonly IProductRepository _repository;
    private readonly IInventoryItemService _inventoryItemService;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper, IInventoryItemService inventoryItemService)
    {
        _repository = repository;
        _mapper = mapper;
        _inventoryItemService = inventoryItemService;
    }
    
    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        var productsDto =  _mapper.Map<IEnumerable<ProductResponse>>(products);
        
        return productsDto;
    }

    public async Task<ProductResponse> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Product code must be greater than 0");
        }
        
        var product = await _repository.GetByIdAsync(id);
        if (product is null)
        {
            throw new NotFoundException("Product not found");
        }
        var productDto =  _mapper.Map<ProductResponse>(product);
        return productDto;
    }

    public async Task<ProductResponse> CreateAsync(ProductRequest request)
    {
        var product = _mapper.Map<Product>(request);
        try 
        {
            await _repository.CreateAsync(product);
        }
        catch (DbUpdateException ex)
        {
            throw new BusinessException("Product with this name already exists");
        }
            
        var productDto = _mapper.Map<ProductResponse>(product);
        return productDto;
    }

    public async Task<ProductResponse> UpdateAsync(int id, string name)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Product code must be greater than 0");
        }
        
        var product = await _repository.GetByIdAsync(id);
        if (product is null)
        {
            throw new NotFoundException("Product not found");
        }
        product.UpdateName(name);
        await _repository.UpdateAsync(product);
            
        var productDto = _mapper.Map<ProductResponse>(product);
        return productDto;
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Product code must be greater than 0");
        }
        
        var product = await _repository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException("Product not found");
    
        var items = await _inventoryItemService.GetItemsByProductIdAsync(id);
        if (items.Any())
            throw new BusinessException("Unable to delete product with in stock items");
    
        await _repository.DeleteAsync(product);
    }
}