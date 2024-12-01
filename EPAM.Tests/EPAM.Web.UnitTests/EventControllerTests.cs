using EPAM.Services.Interfaces;
using EPAM.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;

namespace EPAM.Web.UnitTests
{
    [Category("Unit")]
    public sealed class EventControllerTests
    {
        private readonly Mock<ISeatService> _seatServiceMock;
        private readonly Mock<IEventService> _eventServiceMock;
        private readonly Mock<ILogger<EventsController>> _loggerMock;

        public EventControllerTests()
        {
            _seatServiceMock = new Mock<ISeatService>();
            _eventServiceMock = new Mock<IEventService>();
            _loggerMock = new Mock<ILogger<EventsController>>();
        }

        [Fact]
        public async Task GetAllEventsAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new EventsController(_seatServiceMock.Object, _eventServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetAllEventsAsync(It.IsAny<CancellationToken>());
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _eventServiceMock.Verify(m => m.GetListAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllEventsAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _eventServiceMock.Setup(m => m.GetListAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new EventsController(_seatServiceMock.Object, _eventServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetAllEventsAsync(It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _eventServiceMock.Verify(m => m.GetListAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSeatsAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new EventsController(_seatServiceMock.Object, _eventServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetSeatsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>());
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _seatServiceMock.Verify(m => m.GetByEventIdAndSectionIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSeatsAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _seatServiceMock.Setup(m => m.GetByEventIdAndSectionIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new EventsController(_seatServiceMock.Object, _eventServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetSeatsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _seatServiceMock.Verify(m => m.GetByEventIdAndSectionIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
