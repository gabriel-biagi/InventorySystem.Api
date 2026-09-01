using Xunit;
using InventorySystem.Domain.Entities;

namespace InventorySystem.UnitTests.Domain;

public class InventoryItemTests
{
    
    //AddQuantity
    [Fact]
    public void AddQuantity_WhenQuantityIsNegative_ThrowsArgumentException()
    {
        var product = new Product("Testando", 0);
        var location = new Location(1, 2, 3);
        var inventoryItem = new InventoryItem(product, location);
        
        var ex = Assert.Throws<ArgumentException>(() => inventoryItem.AddQuantity(-1));
        Assert.Equal("Quantity must be greater than zero.", ex.Message);
    }
    
    [Fact]
    public void AddQuantity_WhenQuantityAreValid_AddQuantitySuccessfully()
    {
        var product = new Product("Testando", 0);
        var location = new Location(1, 2, 3);
        var inventoryItem = new InventoryItem(product, location, 10m);
        
        inventoryItem.AddQuantity(10m);
        
        Assert.Equal(20m, inventoryItem.Quantity);
    }
    
    //RemoveQuantity
    [Fact]
    public void RemoveQuantity_WhenQuantityIsNegative_ThrowsArgumentException()
    {
        var product = new Product("Testando", 0);
        var location = new Location(1, 2, 3);
        var inventoryItem = new InventoryItem(product, location);
        
        var ex = Assert.Throws<ArgumentException>(() => inventoryItem.RemoveQuantity(-1m));
        Assert.Equal("Quantity must be greater than zero.", ex.Message);
    }
    
    [Fact]
    public void RemoveQuantity_WhenQuantityIsGreaterThanInStock_ThrowsArgumentException()
    {
        var product = new Product("Testando", 0);
        var location = new Location(1, 2, 3);
        var inventoryItem = new InventoryItem(product, location, 10m);
        
        var ex = Assert.Throws<ArgumentException>(() => inventoryItem.RemoveQuantity(20m));
        Assert.Equal("Insufficient stock quantity.", ex.Message);
    }

    [Fact]
    public void RemoveQuantity_WhenQuantityAreValid_RemoveQuantitySuccessfully()
    {
        var product = new Product("Testando", 0);
        var location = new Location(1, 2, 3);
        var inventoryItem = new InventoryItem(product, location, 10m);
        
        inventoryItem.RemoveQuantity(5m);
        Assert.Equal(5m, inventoryItem.Quantity);
    }
}