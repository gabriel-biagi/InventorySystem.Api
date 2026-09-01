using Xunit;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;

namespace InventorySystem.UnitTests.Domain;

public class ProductTests
{
    [Theory]
    [InlineData("Ab")]
    [InlineData("Abcd")] 
    public void UpdateName_WhenNameIsInvalid_ThrowsArgumentException(string newName)
    {
        var product = new Product("Testee", UnitType.Unit);
        
        var ex = Assert.Throws<ArgumentException>(() => product.UpdateName(newName));
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
    public void UpdateName_WhenNameHasMoreThan80Characters_ThrowsArgumentException()
    {
        var product = new Product("Testee", UnitType.Unit);
        
        var ex = Assert.Throws<ArgumentException>(() => product.UpdateName(new  string('a', 81)));
        Assert.Equal("The product name must be less than 80 characters long.", ex.Message);
    }

    [Theory]
    [InlineData("Testando")]
    [InlineData("A very long product name that has exactly eighty chars total in this string here")]
    [InlineData("Testee")]
    public void UpdateName_WhenNameIsInMinimunAndMaximumCharacterLimit_UpdatesNameSucessfully(string newName)
    {
        var  product = new Product("Initial Name", UnitType.Unit);
        product.UpdateName(newName);
        Assert.Equal(newName, product.Name);
    }
}