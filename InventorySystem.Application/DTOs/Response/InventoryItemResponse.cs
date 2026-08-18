namespace InventorySystem.Application.DTOs.Response;

public class InventoryItemResponse
{
    public int InventoryItemId { get; set; }
    public ProductResponse Product { get; set; }
    public int Column { get; set; }
    public int Shelf { get; set; }
    public int Item { get; set; }
    public decimal Quantity { get; set; }
}