using InventorySystem.Domain.Entities;
using InventorySystem.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Api.Controllers;
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
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _context.Products.ToList();
            if (!products.Any())
            {
                return NotFound("No products found");
            }
            return products;
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product is null)
            {
                return NotFound("No product found");
            }
            return product;
        }

        [HttpPost]
        public ActionResult<Product> PostProduct(Product? product)
        {
            if (product is null)
            {
                return BadRequest("Product is null");
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult DeleteProduct(int id)
        {
            var rowsafected = _context.Products
                .Where(b => b.ProductId == id)
                .ExecuteDelete();
            if (rowsafected == 0)
            {
                return NotFound("No products found");
            }
            return NoContent();
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<Product> PutProduct(int id, string name)
        {
            var product =  _context.Products.Find(id);
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
            _context.SaveChanges();
            return NoContent();
        }

    }
    