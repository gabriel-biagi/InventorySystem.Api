using AutoMapper;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.DTOs.Mappings;

public class InventoryItemResponseMappingProfile : Profile
{
    public InventoryItemResponseMappingProfile()
    {
        CreateMap<InventoryItem, InventoryItemResponse>();
    }
}