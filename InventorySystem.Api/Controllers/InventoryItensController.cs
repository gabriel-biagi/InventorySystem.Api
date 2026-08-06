using InventorySystem.Domain.Entities;
using InventorySystem.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace InventorySystem.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class InventoryItensController : ControllerBase
{
    private  readonly AppDbContext _context;
        
    public InventoryItensController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<InventoryItem>> GetInventoryItems()
    {
        var itens = _context.InventoryItems.Include(b => b.Product)
            .ToList();
        if (itens.Count == 0)
        {
            return NotFound("No products found");
        }
        return itens;
    }

    [HttpPost("products/{productId:int}")]
    public ActionResult<InventoryItem> PostInventoryItem(int productId, [FromBody] InventoryItemRequest request)
    {
        var product = _context.Products.Find(productId);
        if (product is null)
        {
            return NotFound("No product found");
        }

        var location = new Location(request.Column, request.Shelf, request.Item);
        var inventoryItem = new InventoryItem(product, location, request.Quantity);
        
        _context.InventoryItems.Add(inventoryItem);
        _context.SaveChanges();

        return CreatedAtAction(nameof(PostInventoryItem), new { id = inventoryItem.InventoryItemId }, inventoryItem);
    }

    [HttpGet("{id:int}")]
    public ActionResult<InventoryItem> GetInventoryItem(int id)
    {
        var inventoryItem = _context.InventoryItems
            .Include(b => b.Product)
            .FirstOrDefault(b => b.InventoryItemId == id);
        if (inventoryItem is null)
        {
            return NotFound("No products found");
        }
        return inventoryItem;
    }

    [HttpPut("/inventoryitens/{id}/add-quantity")]
    public ActionResult<InventoryItem> PutInventoryItem(int id, decimal quantity)
    {
        var inventoryItem = _context.InventoryItems
            .Find(id);
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
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPut("/inventoryitens/{id}/remove-quantity")]
    public ActionResult<InventoryItem> RemoveInventoryItem(int id, decimal quantity)
    {
        var inventoryItem = _context.InventoryItems
            .Find(id);
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
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult<InventoryItem> DeleteInventoryItem(int id)
    {
        var rowsafected = _context.InventoryItems
            .Where(b => b.InventoryItemId == id)
            .ExecuteDelete();

        if (rowsafected == 0)
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