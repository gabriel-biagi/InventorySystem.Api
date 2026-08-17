using InventorySystem.Api.Filters;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;
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

    public InventoryItensController(IInventoryRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems()
    {
        var itens = await _repository.GetAllAsync();
        return Ok(itens);
    }

    [HttpPost("{productId:int:min(1)}")]
    public async Task<ActionResult<InventoryItem>> PostInventoryItem(int productId, [FromBody] InventoryItemRequest request)
    {
        var product = await _repository.GetProductByIdAsync(productId);
        if (product is null)
        {
            return NotFound("No products found");
        }
        
        var location = new Location(request.Column, request.Shelf, request.Item);
        var inventoryItem = new InventoryItem(product, location, request.Quantity);
        
        var created = await _repository.AddAsync(inventoryItem);

        return CreatedAtAction(nameof(PostInventoryItem), new { id = inventoryItem.InventoryItemId }, inventoryItem);
    }

    [HttpGet("products/{productId:int:min(1)}")]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItemsByProduct(int productId)
    {
        var items = await _repository.GetItemsByProductIdAsync(productId);
        if (!items.Any())
        {
            return NotFound("No items found");
        }
        return Ok(items);
    }

    [HttpPut("{id:int:min(1)}/add-quantity")]
    public async Task<ActionResult<InventoryItem>> PutInventoryItem(int id, decimal quantity)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound("No items found");
        }
        
        item.AddQuantity(quantity);
        await _repository.UpdateAsync(item);
        return Ok(item);
    }

    [HttpPut("{id:int:min(1)}/remove-quantity")]
    public async Task<ActionResult<InventoryItem>> RemoveInventoryItem(int id, decimal quantity)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound("No items found");
        }
        
        item.RemoveQuantity(quantity);
        await _repository.UpdateAsync(item);
        return Ok(item);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<InventoryItem>> DeleteInventoryItemById(int id)
    {
        await _repository.DeleteByIdAsync(id);
        return NoContent();
    }
}
public class InventoryItemRequest
{
    public int Column { get; set; }
    public int Shelf { get; set; }
    public int Item { get; set; }
    public decimal Quantity { get; set; }
}