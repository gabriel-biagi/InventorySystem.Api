using InventorySystem.Api.Filters;
using InventorySystem.Domain.Entities;
using InventorySystem.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Api.Controllers;

    [ServiceFilter(typeof(ApiLoggingFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private  readonly AppDbContext _context;
        
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            if (!products.Any())
            {
                return NotFound("No products found");
            }
            return products;
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound("No product found");
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product? product)
        {
            if (product is null)
            {
                return BadRequest("Product is null");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var rowsafected = await _context.Products
                .Where(b => b.ProductId == id)
                .ExecuteDeleteAsync();
            if (rowsafected == 0)
            {
                return NotFound("No products found");
            }
            return NoContent();
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<Product>> PutProduct(int id, string name)
        {
            var product =  await _context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound("No product found");
            }
            try
            {
                product.UpdateName(name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
    