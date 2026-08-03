using InventorySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<InventoryItem>  InventoryItems { get; set; }
    public DbSet<Product>  Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeia Location como parte do InventoryItem (Owned Type)
        modelBuilder.Entity<InventoryItem>(builder =>
        {
            builder.OwnsOne(i => i.Location);
        });
    }
}