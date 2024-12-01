using EPAM.Services.Interfaces;
using EPAM.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPAM.Web.UnitTests
{
    public sealed class VenueControllerTests
    {
        private readonly Mock<ISectionService> _sectionServiceMock;
        private readonly Mock<IVenueService> _venueServiceMock;
        private readonly Mock<ILogger<VenuesController>> _loggerMock;

        public VenueControllerTests()
        {
            _sectionServiceMock = new Mock<ISectionService>();
            _venueServiceMock = new Mock<IVenueService>();
            _loggerMock = new Mock<ILogger<VenuesController>>();
        }

        [Fact]
        public async Task GetSectionsAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new VenuesController(_sectionServiceMock.Object, _venueServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetSectionsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _sectionServiceMock.Verify(m => m.GetSectionsByVenueId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetPaymentAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _sectionServiceMock.Setup(m => m.GetSectionsByVenueId(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new VenuesController(_sectionServiceMock.Object, _venueServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetSectionsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _sectionServiceMock.Verify(m => m.GetSectionsByVenueId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllVenuesAsync_ShouldRetunOk()
        {
            //Arrange
            var controller = new VenuesController(_sectionServiceMock.Object, _venueServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetAllVenuesAsync(It.IsAny<CancellationToken>());
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _venueServiceMock.Verify(m => m.GetListAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllVenuesAsync_ShouldRetunBadRequest()
        {
            //Arrange
            _venueServiceMock.Setup(m => m.GetListAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            var controller = new VenuesController(_sectionServiceMock.Object, _venueServiceMock.Object, _loggerMock.Object);

            //Act
            var result = await controller.GetAllVenuesAsync(It.IsAny<CancellationToken>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            _venueServiceMock.Verify(m => m.GetListAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
