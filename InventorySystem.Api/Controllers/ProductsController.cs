using InventorySystem.Api.Filters;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;
using InventorySystem.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Api.Controllers;

    [ServiceFilter(typeof(ApiLoggingFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private  readonly IProductRepository _repository;
        
        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound("No product found");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product? product)
        {
            if (product is null)
            {
                return BadRequest("No products found");
            }
            var productCreated = await _repository.CreateAsync(product);
            return CreatedAtAction("PostProduct", new { id = productCreated.ProductId }, productCreated);
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product =  await _repository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound("No product found");
            }
            await _repository.DeleteAsync(product);
            return NoContent();
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<Product>> PutProduct(int id, string name)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound("No product found");
            }
            product.UpdateName(name);
            var  productUpdated = await _repository.UpdateAsync(product);
            return Ok(productUpdated);
        }

    }
    