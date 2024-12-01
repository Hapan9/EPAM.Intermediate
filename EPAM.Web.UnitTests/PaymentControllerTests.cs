using EPAM.Services.Interfaces;
using EPAM.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAM.Web.UnitTests
{
    public sealed class PaymentControllerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly Mock<ILogger<PaymentsController>> _loggerMock;

        public PaymentControllerTests()
        {

            _paymentServiceMock = new Mock<IPaymentService>();
            _loggerMock = new Mock<ILogger<PaymentsController>>();
        }

        [Fact]
        public async Task GetPaymentAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new PaymentsController(_paymentServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetPaymentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _paymentServiceMock.Verify(m => m.GetPaymentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetPaymentAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _paymentServiceMock.Setup(m => m.GetPaymentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new PaymentsController(_paymentServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetPaymentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _paymentServiceMock.Verify(m => m.GetPaymentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusToCompleteAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new PaymentsController(_paymentServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.UpdateStatusToCompleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());
            var okResult = result as OkResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _paymentServiceMock.Verify(m => m.UpdateStatusToCompleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusToCompleteAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _paymentServiceMock.Setup(m => m.UpdateStatusToCompleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new PaymentsController(_paymentServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.UpdateStatusToCompleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _paymentServiceMock.Verify(m => m.UpdateStatusToCompleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusToFailedAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new PaymentsController(_paymentServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.UpdateStatusToFailedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());
            var okResult = result as OkResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _paymentServiceMock.Verify(m => m.UpdateStatusToFailedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusToFailedAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _paymentServiceMock.Setup(m => m.UpdateStatusToFailedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new PaymentsController(_paymentServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.UpdateStatusToFailedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _paymentServiceMock.Verify(m => m.UpdateStatusToFailedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
