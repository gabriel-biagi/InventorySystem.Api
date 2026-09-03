using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.DTOs.Request;

public class LoginRequest
{
    [Required(ErrorMessage = "Registration is required")]
    public int RegistrationNumber { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}