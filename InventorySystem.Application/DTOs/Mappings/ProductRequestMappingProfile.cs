using AutoMapper;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.DTOs.Mappings;

public class ProductRequestMappingProfile :  Profile
{
    public ProductRequestMappingProfile()
    {
        CreateMap<ProductRequest, Product>();
    }
}