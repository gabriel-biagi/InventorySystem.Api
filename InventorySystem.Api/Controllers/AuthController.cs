using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Application.Services.Interfaces;
using InventorySystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
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
        
        var emailExists = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (emailExists is not null)
        {
            return Unauthorized(new RegisterResponse { Success = false, Message = "Email already exists!" });
        }

        ApplicationUser user = new()
        {
            RegistrationNumber = request.RegistrationNumber,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new RegisterResponse { Success = false, Message = "User creation failed." });
        }
        
        return Ok(new RegisterResponse { Success = true, Message = "User created successfully!" });
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (request is null)
        {
            return BadRequest("Invalid client request");
        }

        string? acessToken = request.AccessToken ?? throw new ArgumentNullException(nameof(request));
        string refreshToken = request.RefreshToken ?? throw new ArgumentNullException(nameof(request));

        var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!, _configuration);
        if (principal == null)
        {
            return BadRequest("Invalid access token/refresh token");
        }
        
        string username = principal.Identity.Name;
        
        var user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken 
                         || user.RefreshTokenExpires <= DateTime.UtcNow)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }
    
    [Authorize]
    [HttpPost]
    [Route("revoke")]
    public async Task<IActionResult> Revoke()
    {
        var registrationNumber = User.FindFirst("RegistrationNumber")?.Value;
    
        if (string.IsNullOrEmpty(registrationNumber) || !int.TryParse(registrationNumber, out int regNum))
        {
            return BadRequest("Invalid token");
        }

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.RegistrationNumber == regNum);
    
        if (user is null)
        {
            return BadRequest("User not found");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpires = DateTime.MinValue;
        await _userManager.UpdateAsync(user);
    
        return NoContent();
    }
}
