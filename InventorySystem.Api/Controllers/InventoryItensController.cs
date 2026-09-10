using InventorySystem.Api.Filters;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Application.Services.Interfaces;
using InventorySystem.Domain.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    [Authorize(Policy = "ManagerOrGestor")]
    public async Task<ActionResult<IEnumerable<InventoryItemResponse>>> GetInventoryItems([FromQuery] InventoryItemsParameters inventoryItemsParameters)
    {
        var pagedItems = await _service.GetAllAsync(inventoryItemsParameters);
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(pagedItems.Metadata));
        return Ok(pagedItems.Items);
    }

    [HttpPost("{productId:int:min(1)}")]
    [Authorize(Policy = "ManagerOrGestor")]
    public async Task<ActionResult<InventoryItemResponse>> PostInventoryItem(int productId, [FromBody] InventoryItemRequest request)
    {
        var item = await _service.AddAsync(productId, request);
        return CreatedAtAction(nameof(GetInventoryItem), new { id = item.InventoryItemId }, item);
    }

    [HttpGet("products/{productId:int:min(1)}")]
    [Authorize(Policy = "ManagerOrGestor")]
    public async Task<ActionResult<IEnumerable<InventoryItemResponse>>> GetInventoryItemsByProduct(int productId)
    {
        var items = await _service.GetItemsByProductIdAsync(productId);
        return Ok(items);
    }
    
    [HttpGet("{id:int:min(1)}")]
    [Authorize(Policy = "ManagerOrGestor")]
    public async Task<ActionResult<InventoryItemResponse>> GetInventoryItem(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return Ok(item);
    }

    [HttpPut("{id:int:min(1)}/add-quantity")]
    [Authorize(Policy = "ManagerOrGestor")]
    public async Task<ActionResult<InventoryItemResponse>> PutInventoryItem(int id, decimal quantity)
    {
        var item = await _service.UpdateAsync(id, quantity);
        return Ok(item);
    }

    [HttpPut("{id:int:min(1)}/remove-quantity")]
    [Authorize(Policy = "ManagerOrGestor")]
    public async Task<ActionResult<InventoryItemResponse>> RemoveInventoryItem(int id, decimal quantity)
    {
        var item = await _service.RemoveAsync(id, quantity);
        return Ok(item);
    }

    [HttpDelete("{id:int:min(1)}")]
    [Authorize(Policy = "GestorOnly")]
    public async Task<ActionResult<InventoryItemResponse>> DeleteInventoryItemById(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
