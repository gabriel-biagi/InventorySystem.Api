using System.Text;
using InventorySystem.Api.Extensions;
using InventorySystem.Api.Filters;
using InventorySystem.Application.DTOs.Mappings;
using InventorySystem.Application.Services;
using InventorySystem.Application.Services.Interfaces;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Infrastructure.Context;
using InventorySystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JWT");
var secretKey = jwtSettings["SecretKey"];

if (string.IsNullOrEmpty(secretKey))
    throw new InvalidOperationException("JWT SecretKey não configurado. Configure via 'dotnet user-secrets set \"JWT:SecretKey\" \"sua_chave\"'");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["ValidIssuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["ValidAudience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

var mySqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString)));

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<IInventoryRepository, EfInventoryRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryItemService, InventoryItemService>();

builder.Services.AddAutoMapper(typeof(InventoryItemResponseMappingProfile).Assembly);

var app = builder.Build();

app.ConfigureExceptionHandler(app.Environment);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "InventorySystem API"));
    
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
