using AutoMapper;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.DTOs.Mappings;

public class InventoryItemResponseMappingProfile : Profile
{
    public InventoryItemResponseMappingProfile()
    {
        CreateMap<InventoryItem, InventoryItemResponse>()
            .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.Location.Column))
            .ForMember(dest => dest.Shelf, opt => opt.MapFrom(src => src.Location.Shelf))
            .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Location.Item));
    }
}