using InventorySystem.Domain.Entities;
using InventorySystem.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Api;
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private  readonly AppDbContext _context;
        
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _context.Products.ToList();
            if (!products.Any())
            {
                return NotFound("No products found");
            }
            return products;
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product is null)
            {
                return NotFound("No product found");
            }
            return product;
        }
    }