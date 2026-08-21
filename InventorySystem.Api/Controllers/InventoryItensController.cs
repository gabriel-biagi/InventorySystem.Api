using InventorySystem.Api.Filters;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Api.Controllers;

[ServiceFilter(typeof(ApiLoggingFilter))]
[Route("api/[controller]")]
[ApiController]
public class InventoryItensController : ControllerBase
{
    private  readonly IInventoryItemService _service;

    public InventoryItensController(IInventoryItemService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemResponse>>> GetInventoryItems()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpPost("{productId:int:min(1)}")]
    public async Task<ActionResult<InventoryItemResponse>> PostInventoryItem(int productId, [FromBody] InventoryItemRequest request)
    {
        var item = await _service.AddAsync(productId, request);
        return CreatedAtAction(nameof(GetInventoryItem), new { id = item.InventoryItemId }, item);
    }

    [HttpGet("products/{productId:int:min(1)}")]
    public async Task<ActionResult<IEnumerable<InventoryItemResponse>>> GetInventoryItemsByProduct(int productId)
    {
        var items = await _service.GetItemsByProductIdAsync(productId);
        return Ok(items);
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<InventoryItemResponse>> GetInventoryItem(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return Ok(item);
    }

    [HttpPut("{id:int:min(1)}/add-quantity")]
    public async Task<ActionResult<InventoryItemResponse>> PutInventoryItem(int id, decimal quantity)
    {
        var item = await _service.UpdateAsync(id, quantity);
        return Ok(item);
    }

    [HttpPut("{id:int:min(1)}/remove-quantity")]
    public async Task<ActionResult<InventoryItemResponse>> RemoveInventoryItem(int id, decimal quantity)
    {
        var item = await _service.RemoveAsync(id, quantity);
        return Ok(item);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<InventoryItemResponse>> DeleteInventoryItemById(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
