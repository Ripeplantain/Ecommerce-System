using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using System;
using System.Threading.Tasks;
using Ecommerce.User.Controllers;
using Ecommerce.User.Entities;
using Ecommerce.User.Dto;
using Ecommerce.User.Repository;

namespace Ecommerce.User.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<ILogger<AccountController>> _loggerMock;
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<ITokenRepository> _tokenRepositoryMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;

        private readonly AccountController _controller;

        public AuthControllerTests()
        {
            _loggerMock = new Mock<ILogger<AccountController>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _tokenRepositoryMock = new Mock<ITokenRepository>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();

            _controller = new AccountController(
                _loggerMock.Object,
                _userManagerMock.Object,
                _tokenRepositoryMock.Object,
                _publishEndpointMock.Object
            );
        }

        [Fact]
        public async Task Register_ValidInput_ReturnsCreatedStatus()
        {
            // Arrange
            var input = new UserRegisterDto(
                FullName: "John Doe",
                Email: "test@test.com",
                Password: "password24",
                PhoneNumber: "1234567890"
            );

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Register(input);

            // Assert Statements
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, actionResult.StatusCode);
        }
    }
}