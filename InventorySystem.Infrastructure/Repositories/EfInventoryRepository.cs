using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories;

public class EfInventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _context;

    public EfInventoryRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<InventoryItem>> GetAllAsync()
    {
        var itens = await _context.InventoryItems.Include(b => b.Product)
            .ToListAsync();
        return itens;
    }

    public async Task<InventoryItem?> GetByIdAsync(int id)
    {
        var item = await _context.InventoryItems
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.InventoryItemId == id);
        return item;
    }
    
    public async Task<IEnumerable<InventoryItem>> GetItemsByProductIdAsync(int productId)
    {
        var items = _context.InventoryItems.Include(b => b.Product)
            .Where(b => b.Product.ProductId == productId);
        return await items.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        return product;
    }

    public async Task<InventoryItem> AddAsync(InventoryItem inventoryItem)
    {
        ArgumentNullException.ThrowIfNull(inventoryItem);

        _context.InventoryItems.Add(inventoryItem);
        await _context.SaveChangesAsync();
        return inventoryItem;
    }

    public async Task<InventoryItem> UpdateAsync(InventoryItem inventoryItem)
    {
        ArgumentNullException.ThrowIfNull(inventoryItem);
        
        _context.InventoryItems.Update(inventoryItem);
        await _context.SaveChangesAsync();
        return inventoryItem;
    }

    public async Task DeleteAsync(InventoryItem inventoryItem)
    {
        ArgumentNullException.ThrowIfNull(inventoryItem);
        _context.InventoryItems.Remove(inventoryItem);
        await _context.SaveChangesAsync();
    }
}