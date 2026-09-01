using Xunit;
using Moq;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Application.Services;
using InventorySystem.Domain.Exception;
using AutoMapper;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Application.Services.Interfaces;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;

namespace InventorySystem.UnitTests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task GetByIdAsync_WhenIdIsInvalid_ThrowsArgumentException()
    {
        var mockRepo = new Mock<IProductRepository>();
        var mockMapper = new Mock<IMapper>();
        
        var service = new ProductService(mockRepo.Object, mockMapper.Object, null);
        
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetByIdAsync(-1));
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductIsNull_ThrowsNotFoundException()
    {
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product) null);
        var mockMapper = new Mock<IMapper>();
        var service = new ProductService(mockRepo.Object, mockMapper.Object, null);
        
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(1));
    }
    
    [Fact]
    public async Task DeleteAsync_WhenProductIsInStock_ThrowsBusinessException()
    {
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Product("Testando", UnitType.Unit));
        var  mockMapper = new Mock<IMapper>();
        var mockInventoryService = new Mock<IInventoryItemService>();
        mockInventoryService.Setup(r => r.GetItemsByProductIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<InventoryItemResponse> { new InventoryItemResponse() }.AsEnumerable());
        
        var service = new ProductService(mockRepo.Object, mockMapper.Object, mockInventoryService.Object);
        
        await Assert.ThrowsAsync<BusinessException>(() => service.DeleteAsync(1));
    }
}