using InventorySystem.Domain;

namespace InventorySystem.Application.DTOs.Response;

public class ProductResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public UnitType UnitType { get; set; }
}