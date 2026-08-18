using AutoMapper;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.DTOs.Mappings;

public class ProductResponseMappingProfile : Profile
{
    public ProductResponseMappingProfile()
    {
        CreateMap<Product, ProductResponse>();
    }
}