namespace InventorySystem.Application.DTOs.Response;

public class RegisterResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public TokenResponse? Token { get; set; }
}