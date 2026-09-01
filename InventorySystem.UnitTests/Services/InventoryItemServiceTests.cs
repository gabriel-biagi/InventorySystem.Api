using Xunit;
using Moq;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Application.Services;
using InventorySystem.Domain.Exception;
using AutoMapper;
using InventorySystem.Domain.Entities;

namespace InventorySystem.UnitTests.Services;

public class InventoryItemServiceTests
{
    [Fact]
    public async Task UpdateAsync_WhenQuantityIsNotIntenger_ThrowsBusinessException()
    {
        var mockRepo =  new Mock<IInventoryRepository>();
        var product = new Product("Teste", 0);
        mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new InventoryItem(product, null, 1.5m));
        var service = new InventoryItemService(mockRepo.Object, null, null);
        
        var ex = await Assert.ThrowsAsync<BusinessException>(() => service.UpdateAsync(1, 1.5m));
        Assert.Equal("Quantity must be integer for Unit or Package", ex.Message);
    }
}