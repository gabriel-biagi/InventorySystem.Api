using AutoMapper;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.DTOs.Mappings;

public class InventoryItemRequestMappingProfile : Profile
{
    public  InventoryItemRequestMappingProfile()
    {
        CreateMap<InventoryItemRequest, InventoryItem>();
    }
}