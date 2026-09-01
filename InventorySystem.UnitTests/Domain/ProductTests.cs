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
        
        Assert.Throws<ArgumentException>(() => product.UpdateName("A"));
    }

    [Fact]
    public void UpdateName_WhenNameIsWhiteSpace_ThrowsArgumentException()
    {
        var product = new Product("Testee", UnitType.Unit);
        Assert.Throws<ArgumentException>(() => product.UpdateName(" "));
    }
    
    [Fact]
    public void UpdateName_WhenNameIsNull_ThrowsArgumentException()
    {
        var product = new Product("Testee", UnitType.Unit);
        Assert.Throws<ArgumentException>(() => product.UpdateName(null));
    }

    [Fact]
    public void UpdateName_WhenNameAreValid_UpdatesNameSucessfully()
    {
        var product = new Product("Testee", UnitType.Unit);
        product.UpdateName("Testando");
        Assert.Equal("Testando", product.Name);
    }
}