using Ecommerce.Common;
using Ecommerce.Catalog.Entities;
using Ecommerce.Catalog.Dtos;
using Ecommerce.Catalog.Contract;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using MassTransit;


namespace Ecommerce.Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        private readonly ILogger<ProductController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
    
        public ProductController(
            IRepository<Product> repository,
            ILogger<ProductController> logger,
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery] string? filter = null
        )
        {
            try {
                Expression<Func<Product, bool>> filterExpression = product => true;
                if (!string.IsNullOrEmpty(filter))
                {
                    filterExpression = product => product.Name.Contains(filter);
                }

                var products = (await _repository.GetAllAsync(filterExpression))
                    .Select(product => product.AsDto());

                return Ok(products);
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            try {
                var product = await _repository.GetByIdAsync(id);

                if (product is null)
                {
                    return NotFound();
                }

                return Ok(product.AsDto());
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDto createProductDto)
        {
            try {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = createProductDto.Name,
                    Description = createProductDto.Description,
                    Price = createProductDto.Price,
                    Quantity = createProductDto.Quantity,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _repository.CreateAsync(product);
                await _publishEndpoint.Publish(new ProductCreated(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price
                ));

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product.AsDto());
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, UpdateProductDto updateProductDto)
        {
            try {
                var existingProduct = await _repository.GetByIdAsync(id);

                if (existingProduct is null)
                {
                    return NotFound();
                }

                existingProduct.Name = updateProductDto.Name;
                existingProduct.Description = updateProductDto.Description;
                existingProduct.Price = updateProductDto.Price;
                existingProduct.Quantity = updateProductDto.Quantity;

                await _repository.UpdateAsync(existingProduct);
                await _publishEndpoint.Publish(new ProductUpdated(
                    existingProduct.Id,
                    existingProduct.Name,
                    existingProduct.Description,
                    existingProduct.Price
                ));

                return NoContent();
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            try {
                var existingProduct = await _repository.GetByIdAsync(id);
    
                if (existingProduct is null)
                {
                    return NotFound();
                }
    
                await _repository.DeleteAsync(existingProduct.Id);
                await _publishEndpoint.Publish(new ProductDeleted(existingProduct.Id));
    
                return NoContent();
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}