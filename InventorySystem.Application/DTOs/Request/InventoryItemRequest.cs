using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Application.DTOs.Request;

public class InventoryItemRequest
{
    public int Column { get; set; }
    public int Shelf { get; set; }
    public int Item { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Quantity { get; set; }
}