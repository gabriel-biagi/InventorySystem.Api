using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Application.Services.Interfaces;
using InventorySystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.RegistrationNumber == request.RegistrationNumber);
    
        if (user == null)
            return Unauthorized(new LoginResponse { Success = false, Message = "Invalid credentials" });
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
    
        if (!isPasswordValid)
            return Unauthorized(new LoginResponse { Success = false, Message = "Invalid credentials" });
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim("RegistrationNumber", user.RegistrationNumber.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        
        var accessToken = _tokenService.GenerateAccessToken(authClaims, _configuration);
        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);
        
        await _userManager.UpdateAsync(user);
    
        return Ok(new LoginResponse 
        { 
            Success = true,
            Message = "Login successful",
            Token = new TokenResponse 
            { 
                AccessToken = accessTokenString,
                RefreshToken = refreshToken 
            }
        });
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var userExists =
            await _userManager.Users.FirstOrDefaultAsync(u => u.RegistrationNumber == request.RegistrationNumber);

        if (userExists is not null)
        {
            return Unauthorized(new RegisterResponse { Success = false, Message = "User already exists!" });
        }

        ApplicationUser user = new()
        {
            RegistrationNumber = request.RegistrationNumber,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = request.UserName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new RegisterResponse { Success = false, Message = "User creation failed." });
        }
        
        return Ok(new RegisterResponse { Success = true, Message = "User created successfully!" });
    }
}