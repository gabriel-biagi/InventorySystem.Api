
using InventorySystem.Api.Filters;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Api.Controllers;

    [ServiceFilter(typeof(ApiLoggingFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public async Task<ActionResult<ProductResponse>> GetProductById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> PostProduct([FromBody] ProductRequest? request)
        {
            if (request is null)
            {
                return BadRequest("Invalid request");
            }
            
            var product =  await _service.CreateAsync(request);
            return Ok(product);
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<ProductResponse>> PutProduct(int id, string name)
        {
            var product = await _service.UpdateAsync(id, name);
            return Ok(product);
        }

    }
    