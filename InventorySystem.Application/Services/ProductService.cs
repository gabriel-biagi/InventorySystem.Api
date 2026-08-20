using AutoMapper;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Application.Services.Interfaces;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Exception;
using InventorySystem.Domain.Interfaces;

namespace InventorySystem.Application.Services;

public class ProductService : IProductService
{
    private  readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        var productsDto =  _mapper.Map<IEnumerable<ProductResponse>>(products);
        
        return productsDto;
    }

    public async Task<ProductResponse> GetByIdAsync(int id)
    {
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
        await _repository.CreateAsync(product);
            
        var productDto = _mapper.Map<ProductResponse>(product);
        return productDto;
    }

    public async Task<ProductResponse> UpdateAsync(int id, string name)
    {
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
        var product =  await _repository.GetByIdAsync(id);
        if (product is null)
        {
            throw new NotFoundException("Product not found");
        }
        await _repository.DeleteAsync(product);
    }
}