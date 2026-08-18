using AutoMapper;
using InventorySystem.Api.Filters;
using InventorySystem.Application.DTOs.Request;
using InventorySystem.Application.DTOs.Response;
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
        private readonly IMapper _mapper;
        
        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            var products = await _repository.GetAllAsync();
            
            var productsDto =  _mapper.Map<IEnumerable<ProductResponse>>(products);
            return Ok(productsDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public async Task<ActionResult<ProductResponse>> GetProductById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound("No product found");
            }
            
            var productDto =  _mapper.Map<ProductResponse>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> PostProduct([FromBody] ProductRequest request)
        {
            if (request is null)
            {
                return BadRequest("Invalid request");
            }
            
            var product = _mapper.Map<Product>(request);
            await _repository.CreateAsync(product);
            
            var productDto = _mapper.Map<ProductResponse>(product);
            return CreatedAtAction(nameof(GetProductById), new { id = productDto.ProductId }, productDto);
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
        public async Task<ActionResult<ProductResponse>> PutProduct(int id, string name)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound("No product found");
            }
            product.UpdateName(name);
            await _repository.UpdateAsync(product);
            
            var productDto = _mapper.Map<ProductResponse>(product);
            return Ok(productDto);
        }

    }
    