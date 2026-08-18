using AutoMapper;
using InventorySystem.Api.Filters;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Application.DTOs;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace InventorySystem.Api.Controllers;

[ServiceFilter(typeof(ApiLoggingFilter))]
[Route("api/[controller]")]
[ApiController]
public class InventoryItensController : ControllerBase
{
    private  readonly IInventoryRepository _repository;
    private readonly IMapper _mapper;

    public InventoryItensController(IInventoryRepository repository,  IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemResponse>>> GetInventoryItems()
    {
        var items = await _repository.GetAllAsync();
        var itemsDto = _mapper.Map<IEnumerable<InventoryItemResponse>>(items);
        return Ok(itemsDto);
    }

    [HttpPost("{productId:int:min(1)}")]
    public async Task<ActionResult<InventoryItemResponse>> PostInventoryItem(int productId, [FromBody] InventoryItemRequest request)
    {
        var product = await _repository.GetProductByIdAsync(productId);
        if (product is null)
        {
            return NotFound("No products found");
        }
        
        var location = new Location(request.Column, request.Shelf, request.Item);
        var inventoryItem = new InventoryItem(product, location, request.Quantity);
        
        await _repository.AddAsync(inventoryItem);

        var inventoryItemDto = _mapper.Map<InventoryItemResponse>(inventoryItem);
        return CreatedAtAction(nameof(GetInventoryItem), new { id = inventoryItemDto.InventoryItemId }, inventoryItemDto);
    }

    [HttpGet("products/{productId:int:min(1)}")]
    public async Task<ActionResult<IEnumerable<InventoryItemResponse>>> GetInventoryItemsByProduct(int productId)
    {
        var items = await _repository.GetItemsByProductIdAsync(productId);
        if (!items.Any())
        {
            return NotFound("No items found");
        }
        
        var itemsDto = _mapper.Map<IEnumerable<InventoryItemResponse>>(items);
        return Ok(itemsDto);
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<InventoryItemResponse>> GetInventoryItem(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound("No items found");
        }
        var itemDto = _mapper.Map<InventoryItemResponse>(item);
        return Ok(itemDto);
    }

    [HttpPut("{id:int:min(1)}/add-quantity")]
    public async Task<ActionResult<InventoryItemResponse>> PutInventoryItem(int id, decimal quantity)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound("No items found");
        }
        
        item.AddQuantity(quantity);
        await _repository.UpdateAsync(item);
        
        var itemDto = _mapper.Map<InventoryItemResponse>(item);
        return Ok(itemDto);
    }

    [HttpPut("{id:int:min(1)}/remove-quantity")]
    public async Task<ActionResult<InventoryItemResponse>> RemoveInventoryItem(int id, decimal quantity)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound("No items found");
        }
        
        item.RemoveQuantity(quantity);
        await _repository.UpdateAsync(item);
        
        var itemDto = _mapper.Map<InventoryItemResponse>(item);
        return Ok(itemDto);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<InventoryItemResponse>> DeleteInventoryItemById(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound("No items found");
        }
        await _repository.DeleteAsync(item);
        return NoContent();
    }
}
