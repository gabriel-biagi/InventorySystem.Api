using System.ComponentModel.DataAnnotations;
using InventorySystem.Domain;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Application.DTOs.Request;

public class ProductRequest
{
    [Required]
    [StringLength(80)]
    public string Name { get; set; }
    [Required]
    public UnitType UnitType { get; set; }
}