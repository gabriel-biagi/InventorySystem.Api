namespace InventorySystem.Application.DTOs.Request;

public class InventoryItemRequest
{
    public int Column { get; set; }
    public int Shelf { get; set; }
    public int Item { get; set; }
    public decimal Quantity { get; set; }
}