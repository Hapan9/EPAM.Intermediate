using EPAM.Services.Dtos.Order;
using EPAM.Services.Interfaces;
using EPAM.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAM.Web.UnitTests
{
    public sealed class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Mock<ILogger<OrdersController>> _loggerMock;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _loggerMock = new Mock<ILogger<OrdersController>>();
        }

        [Fact]
        public async Task GetOrdersByCartIdAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new OrdersController(_orderServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetOrdersByCartIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _orderServiceMock.Verify(m => m.GetOrdersByCartIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetOrdersByCartIdAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _orderServiceMock.Setup(m => m.GetOrdersByCartIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new OrdersController(_orderServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetOrdersByCartIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _orderServiceMock.Verify(m => m.GetOrdersByCartIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrdersForCartAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new OrdersController(_orderServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.CreateOrdersForCartAsync(It.IsAny<Guid>(), It.IsAny<CreateOrderDto>(), It.IsAny<CancellationToken>());
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _orderServiceMock.Verify(m => m.CreateOrderAsync(It.IsAny<Guid>(), It.IsAny<CreateOrderDto>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrdersForCartAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _orderServiceMock.Setup(m => m.CreateOrderAsync(It.IsAny<Guid>(), It.IsAny<CreateOrderDto>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new OrdersController(_orderServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.CreateOrdersForCartAsync(It.IsAny<Guid>(), It.IsAny<CreateOrderDto>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _orderServiceMock.Verify(m => m.CreateOrderAsync(It.IsAny<Guid>(), It.IsAny<CreateOrderDto>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOrdersForCartAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new OrdersController(_orderServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.DeleteOrdersForCartAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>());
            var okResult = result as OkResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _orderServiceMock.Verify(m => m.DeleteOrderAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOrdersForCartAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _orderServiceMock.Setup(m => m.DeleteOrderAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new OrdersController(_orderServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.DeleteOrdersForCartAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _orderServiceMock.Verify(m => m.DeleteOrderAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task BookSeatsForCartAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new OrdersController(_orderServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.BookSeatsForCartAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _orderServiceMock.Verify(m => m.BookAllSeatsAsyc(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task BookSeatsForCartAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _orderServiceMock.Setup(m => m.BookAllSeatsAsyc(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new OrdersController(_orderServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.BookSeatsForCartAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _orderServiceMock.Verify(m => m.BookAllSeatsAsyc(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
