using System.Linq.Expressions;
using Ecommerce.Catalog.Controllers;
using Ecommerce.Catalog.Dtos;
using Ecommerce.Catalog.Entities;
using Ecommerce.Common;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace Ecommerce.Catalog.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IRepository<Product>> _repositoryMock;
        private readonly Mock<ILogger<ProductController>> _loggerMock;
        private readonly ProductController _controller;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;

        public ProductControllerTests()
        {
            _repositoryMock = new Mock<IRepository<Product>>();
            _loggerMock = new Mock<ILogger<ProductController>>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _controller = new ProductController(_repositoryMock.Object, _loggerMock.Object, _publishEndpointMock.Object);
        }

        [Fact]
        public async Task GetProducts_WithNoFilter_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1" },
                new Product { Id = Guid.NewGuid(), Name = "Product 2" },
                new Product { Id = Guid.NewGuid(), Name = "Product 3" }
            };
            _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Equal(products.Count, returnedProducts.Count());
        }

        // Add more test methods for other controller actions...

        // BEGIN: GetProduct_WithValidId_ReturnsProduct
        [Fact]
        public async Task GetProduct_WithValidId_ReturnsProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Product 1" };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(product.Id, returnedProduct.Id);
            Assert.Equal(product.Name, returnedProduct.Name);
        }
        // END: GetProduct_WithValidId_ReturnsProduct

        // BEGIN: CreateProduct_WithValidProduct_ReturnsCreatedProduct
        [Fact]
        public async Task CreateProduct_WithValidProduct_ReturnsCreatedProduct()
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
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductDto>(createdResult.Value);
        }

        // Begin: Update Product
        [Fact]
        public async Task UpdateProduct_WithValidProduct_ReturnsUpdatedProduct()
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

            var newProduct = new UpdateProductDto(product.Name, product.Description, product.Price, product.Quantity);
            _repositoryMock.Setup(repo => repo.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.UpdateProduct(product.Id, newProduct);

            // Assert
            var okResult = Assert.IsType<NoContentResult>(result);
        }

        // Begin: Delete Product
        [Fact]
        public async Task DeleteProduct_WithValidProduct_ReturnsNoContent()
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

            _repositoryMock.Setup(repo => repo.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.DeleteProduct(product.Id);

            // Assert
            var okResult = Assert.IsType<NoContentResult>(result);
        }
    }
}