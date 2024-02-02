using Ecommerce.Notification.Contract;
using Ecommerce.Order.Controllers;
using Ecommerce.Order.Dtos;
using Ecommerce.Order.Entities;
using Ecommerce.Order.Repository;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;



namespace Ecommerce.Order.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IModelRepository> _modelRepositoryMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly OrderController _orderController;

        public OrderControllerTests()
        {
            _modelRepositoryMock = new Mock<IModelRepository>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _orderController = new OrderController(_modelRepositoryMock.Object, _publishEndpointMock.Object);
        }

        [Fact]
        public async Task GetProducts_WithoutFilter_ReturnsOkResult()
        {
            // Arrange
            var expectedOrders = new List<OrderDto> { 
                new OrderDto(1, Guid.NewGuid(), Guid.NewGuid(), 1, 100, "status", "address", DateTime.Now),
                new OrderDto(2, Guid.NewGuid(), Guid.NewGuid(), 1, 100, "status", "address", DateTime.Now)
            };
            _modelRepositoryMock.Setup(repo => repo.GetOrdersAsync()).ReturnsAsync(expectedOrders);

            // Act
            var result = await _orderController.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetProduct_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var expectedOrder = new OrderDto(1, Guid.NewGuid(), Guid.NewGuid(), 1, 100, "status", "address", DateTime.Now);
            _modelRepositoryMock.Setup(repo => repo.GetOrderAsync(It.IsAny<int>())).ReturnsAsync(expectedOrder);

            // Act
            var result = await _orderController.GetProduct(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateOrder_WithValidOrder_ReturnsOkResult()
        {
            // Arrange
            var createOrder = new CreateOrderDto(Guid.NewGuid(), Guid.NewGuid(), 1, 100, "status", "address");
            var expectedOrder = new OrderDto(1, createOrder.UserId, createOrder.ProductId, createOrder.Quantity, createOrder.Price, createOrder.Status, createOrder.Address, DateTime.Now);
            _modelRepositoryMock.Setup(repo => repo.CreateOrderAsync(It.IsAny<CreateOrderDto>())).ReturnsAsync(expectedOrder);

            // Act
            var result = await _orderController.CreateOrder(createOrder);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateOrder_WithValidOrder_ReturnsOkResult()
        {
            // Arrange
            var updateOrder = new UpdateOrderDto(Guid.NewGuid(), Guid.NewGuid(), 1, 100, "status", "address");

            // Act
            var result = await _orderController.UpdateOrder(1, updateOrder);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_WithInvalidOrder_ReturnsBadRequestResult()
        {
            // Arrange
            var updateOrder = new UpdateOrderDto(Guid.NewGuid(), Guid.NewGuid(), 1, 100, "status", "address");
            _modelRepositoryMock.Setup(repo => repo.UpdateOrderAsync(It.IsAny<int>(), It.IsAny<UpdateOrderDto>())).Throws(new Exception());

            // Act
            var result = await _orderController.UpdateOrder(1, updateOrder);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetProducts_WithFilter_ReturnsOkResult()
        {
            // Arrange
            var filter = "status";
            var expectedOrders = new List<OrderDto> { 
                new OrderDto(1, Guid.NewGuid(), Guid.NewGuid(), 1, 100, "status", "address", DateTime.Now),
                new OrderDto(2, Guid.NewGuid(), Guid.NewGuid(), 1, 100, "status", "address", DateTime.Now)
            };
            _modelRepositoryMock.Setup(repo => repo.GetOrdersAsync(filter)).ReturnsAsync(expectedOrders);

            // Act
            var result = await _orderController.GetProducts(filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetProducts_WithFilter_ReturnsBadRequestResult()
        {
            // Arrange
            var filter = "status";
            _modelRepositoryMock.Setup(repo => repo.GetOrdersAsync(filter)).Throws(new Exception());

            // Act
            var result = await _orderController.GetProducts(filter);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetProduct_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            _modelRepositoryMock.Setup(repo => repo.GetOrderAsync(It.IsAny<int>())).Throws(new Exception());

            // Act
            var result = await _orderController.GetProduct(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateOrder_WithInvalidOrder_ReturnsBadRequestResult()
        {
            // Arrange
            var createOrder = new CreateOrderDto(Guid.NewGuid(), Guid.NewGuid(), 1, 100, "status", "address");
            _modelRepositoryMock.Setup(repo => repo.CreateOrderAsync(It.IsAny<CreateOrderDto>())).Throws(new Exception());

            // Act
            var result = await _orderController.CreateOrder(createOrder);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteOrder_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _orderController.DeleteOrder(id);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
        }
    }
}