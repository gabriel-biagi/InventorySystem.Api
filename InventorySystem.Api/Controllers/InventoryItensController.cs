using InventorySystem.Domain.Entities;
using InventorySystem.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace InventorySystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryItensController : ControllerBase
{
    private  readonly AppDbContext _context;
        
    public InventoryItensController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems()
    {
        var itens = await _context.InventoryItems.Include(b => b.Product)
            .ToListAsync();
        if (itens.Count == 0)
        {
            return NotFound("No products found");
        }
        return itens;
    }

    [HttpPost("products/{productId:int:min(1)}")]
    public async Task<ActionResult<InventoryItem>> PostInventoryItem(int productId, [FromBody] InventoryItemRequest request)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product is null)
        {
            return NotFound("No product found");
        }

        var location = new Location(request.Column, request.Shelf, request.Item);
        var inventoryItem = new InventoryItem(product, location, request.Quantity);
        
        _context.InventoryItems.Add(inventoryItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(PostInventoryItem), new { id = inventoryItem.InventoryItemId }, inventoryItem);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<InventoryItem>> GetInventoryItem(int id)
    {
        var inventoryItem = await _context.InventoryItems
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.InventoryItemId == id);
        if (inventoryItem is null)
        {
            return NotFound("No products found");
        }
        return inventoryItem;
    }

    [HttpPut("{id:int:min(1)}/add-quantity")]
    public async Task<ActionResult<InventoryItem>> PutInventoryItem(int id, decimal quantity)
    {
        var inventoryItem = await _context.InventoryItems
            .FindAsync(id);
        if (inventoryItem is null)
        {
            return NotFound("No products found");
        }

        try
        {
            inventoryItem.AddQuantity(quantity);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:int:min(1)}/remove-quantity")]
    public async Task<ActionResult<InventoryItem>> RemoveInventoryItem(int id, decimal quantity)
    {
        var inventoryItem = await _context.InventoryItems
            .FindAsync(id);
        if (inventoryItem is null)
        {
            return NotFound("No products found");
        }

        try
        {
            inventoryItem.RemoveQuantity(quantity);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<InventoryItem>> DeleteInventoryItem(int id)
    {
        var affected = await _context.InventoryItems
            .Where(b => b.InventoryItemId == id)
            .ExecuteDeleteAsync();

        if (affected == 0)
        {
            return NotFound("No products found");
        }
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