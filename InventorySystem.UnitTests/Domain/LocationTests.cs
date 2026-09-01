using Xunit;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;

namespace InventorySystem.UnitTests.Domain;

public class LocationTests
{
    [Fact]
    public void CreateLocation_WhenLocationLessThan0_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Location(1, 2, -1));
    }
    
    [Fact]
    public void CreateLocation_WhenAllValuesAreValid_CreatesSuccessfully()
    {
        var location = new Location(1, 2, 3);
        
        Assert.NotNull(location);
        Assert.Equal(1, location.Column);
        Assert.Equal(2, location.Shelf);
        Assert.Equal(3, location.Item);
    }
}