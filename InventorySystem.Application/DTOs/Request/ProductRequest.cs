using System.ComponentModel.DataAnnotations;
using InventorySystem.Domain;

namespace InventorySystem.Application.DTOs.Request;

public class ProductRequest
{
    [Required]
    [StringLength(80)]
    public string Name { get; set; }
    [Required]
    public UnitType UnitType { get; set; }
}