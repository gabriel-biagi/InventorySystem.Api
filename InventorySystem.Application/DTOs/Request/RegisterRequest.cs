using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.DTOs.Request;

public class RegisterRequest
{
    [Required(ErrorMessage = "Registration is required")]
    public int RegistrationNumber { get; set; }
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}