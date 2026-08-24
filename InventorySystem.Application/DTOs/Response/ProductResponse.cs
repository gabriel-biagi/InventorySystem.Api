using InventorySystem.Domain;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Application.DTOs.Response;

public class ProductResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public UnitType UnitType { get; set; }
}