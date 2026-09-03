using Microsoft.AspNetCore.Identity;

namespace InventorySystem.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public int RegistrationNumber { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
}