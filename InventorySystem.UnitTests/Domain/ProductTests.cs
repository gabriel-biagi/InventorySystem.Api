using Xunit;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;

namespace InventorySystem.UnitTests.Domain;

public class ProductTests
{
    [Fact]
    public void UpdateName_WhenNameIsInvalid_ThrowsArgumentException()
    {
        var product = new Product("Testee", UnitType.Unit);
        
        var ex = Assert.Throws<ArgumentException>(() => product.UpdateName("A"));
        Assert.Equal("The product name must be at least 5 characters long.", ex.Message);
    }

    [Fact]
    public void UpdateName_WhenNameIsWhiteSpace_ThrowsArgumentException()
    {
        var product = new Product("Testee", UnitType.Unit);
        var ex = Assert.Throws<ArgumentException>(() => product.UpdateName(" "));
        Assert.Equal("The product name must be at least 5 characters long.", ex.Message);
    }
    
    [Fact]
    public void UpdateName_WhenNameIsNull_ThrowsArgumentException()
    {
        var product = new Product("Testee", UnitType.Unit);
        var ex = Assert.Throws<ArgumentException>(() => product.UpdateName(null));
        Assert.Equal("The product name must be at least 5 characters long.", ex.Message);
    }

    [Fact]
    public void UpdateName_WhenNameAreValid_UpdatesNameSucessfully()
    {
        var product = new Product("Testee", UnitType.Unit);
        product.UpdateName("Testando");
        Assert.Equal("Testando", product.Name);
    }
}