
using InventorySystem.Api.Filters;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
using InventorySystem.Application.Services.Interfaces;
using InventorySystem.Domain.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [Authorize(Policy = "ManagerOrGestor")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts([FromQuery] ProductParameters productParameters)
        {
            var pagedProducts = await _service.GetAllAsync(productParameters);
    
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(pagedProducts.Metadata));
    
            return Ok(pagedProducts.Items);
        }

        
        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        [Authorize(Policy = "ManagerOrGestor")]
        public async Task<ActionResult<ProductResponse>> GetProductById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Policy = "GestorOnly")]
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
        [Authorize(Policy = "GestorOnly")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id:int:min(1)}")]
        [Authorize(Policy = "ManagerOrGestor")]
        public async Task<ActionResult<ProductResponse>> PutProduct(int id, string name)
        {
            var product = await _service.UpdateAsync(id, name);
            return Ok(product);
        }

    }
    