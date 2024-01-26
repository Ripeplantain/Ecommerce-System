using Ecommerce.Catalog.Controllers;
using Ecommerce.Catalog.Dtos;
using Ecommerce.Catalog.Entities;
using Ecommerce.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Ecommerce.Catalog.UnitTests.Controllers
{
    public class ProductControllerTests
    {

        private readonly Mock<IRepository<Product>> _repositoryMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _repositoryMock = new Mock<IRepository<Product>>();
            _controller = new ProductController(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult()
        {
            // Arrange
            var products = new List<Product> { /* create some test products */ };
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Equal(products.Count, returnedProducts.Count());
        }

        [Fact]
        public async Task GetProduct_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { /* create a test product with the given id */ };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(product.Id, returnedProduct.Id);
        }

        [Fact]
        public async Task CreateProduct_WithValidProduct_ReturnsOkResult()
        {
            // Arrange
            var product = new Product {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                Quantity = 10,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            var newProduct = new CreateProductDto(product.Name, product.Description, product.Price, product.Quantity);

            // Act
            var result = await _controller.CreateProduct(newProduct);

            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_WithValidProduct_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                Quantity = 10,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            var updatedProduct = new UpdateProductDto(product.Name, product.Description, product.Price, product.Quantity);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.UpdateProduct(productId, updatedProduct);

            // Assert
            var okResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                Quantity = 10,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            var okResult = Assert.IsType<NoContentResult>(result);
        }
    }
}