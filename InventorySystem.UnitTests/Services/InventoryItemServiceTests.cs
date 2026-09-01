using Xunit;
using Moq;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Application.Services;
using InventorySystem.Domain.Exception;
using AutoMapper;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;

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
    
    [Fact]
    public async Task AddAsync_WhenParametersAreValid_AddAsyncSuccessfully()
    {
        var mockRepo =  new Mock<IInventoryRepository>();
        mockRepo.Setup(r => r.GetItemsByProductIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<InventoryItem>());
        var mockRepoProduct = new Mock<IProductRepository>();
        mockRepoProduct.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Product("Product Test", UnitType.Unit));
        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<InventoryItemResponse>(It.IsAny<InventoryItem>()))
            .Returns(new InventoryItemResponse
            {
                InventoryItemId = 1,
                Product = null,
                Column = 1,
                Shelf = 2,
                Item = 3,
                Quantity = 10m
            });
        var request = new InventoryItemRequest
        {
            Column = 1,
            Shelf = 2,
            Item = 3,
            Quantity = 10m
        };
        var service = new InventoryItemService(mockRepo.Object, mockMapper.Object, mockRepoProduct.Object);

        var result = await service.AddAsync(1, request);
        
        Assert.NotNull(result);
        Assert.Equal(1, result.Column);
        Assert.Equal(2, result.Shelf);
        Assert.Equal(3, result.Item);
        Assert.Equal(10m, result.Quantity);
    }
}