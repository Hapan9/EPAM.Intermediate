using AutoFixture;
using EPAM.EF.Entities;
using EPAM.EF.Interfaces;
using EPAM.EF.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MockQueryable.Moq;
using Moq;
using System.Linq.Expressions;

namespace EPAM.EF.UnitTests.RepositoriesTests
{
    public class BaseRepositoryTests
    {
        private readonly Mock<ISystemContext> _systemContextMock;

        public BaseRepositoryTests()
        {
            _systemContextMock = new Mock<ISystemContext>();
        }

        [Fact]
        public async Task CreateAsync_ShouldCallAddAsync()
        {
            //Arrange
            var fixture = new Fixture();

            var venues = fixture.Build<Venue>()
                    .Without(v => v.Sections)
                    .Without(v => v.Events)
                    .CreateMany(2);

            var mockDbSet = venues.BuildMockDbSet();
            _systemContextMock.Setup(c => c.GetDbSet<Venue>()).Returns(mockDbSet.Object);

            var repository = new BaseRepository<Venue>(_systemContextMock.Object);

            //Act
            await repository.CreateAsync(It.IsAny<Venue>(), It.IsAny<CancellationToken>());

            //Assert
            _systemContextMock.Verify(c => c.GetDbSet<Venue>(), Times.Once);
            mockDbSet.Verify(m => m.AddAsync(It.IsAny<Venue>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveRange_WhenEntetiesExist()
        {
            //Arrange
            var fixture = new Fixture();

            var venues = fixture.Build<Venue>()
                    .Without(v => v.Sections)
                    .Without(v => v.Events)
                    .CreateMany(2);

            var mockDbSet = venues.BuildMockDbSet();
            _systemContextMock.Setup(c => c.GetDbSet<Venue>()).Returns(mockDbSet.Object);
            Expression<Func<Venue, bool>> expression = binding => true;

            var repository = new BaseRepository<Venue>(_systemContextMock.Object);

            //Act
            await repository.DeleteAsync(expression, It.IsAny<CancellationToken>());

            //Assert
            _systemContextMock.Verify(c => c.GetDbSet<Venue>(), Times.Exactly(2));
            mockDbSet.Verify(m => m.RemoveRange(It.IsAny<IEnumerable<Venue>>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotRemoveRange_WhenEntetiesNotExist()
        {
            //Arrange
            var fixture = new Fixture();

            var venues = fixture.Build<Venue>()
                    .Without(v => v.Sections)
                    .Without(v => v.Events)
                    .CreateMany(2);

            var mockDbSet = venues.BuildMockDbSet();
            _systemContextMock.Setup(c => c.GetDbSet<Venue>()).Returns(mockDbSet.Object);
            Expression<Func<Venue, bool>> expression = binding => false;

            var repository = new BaseRepository<Venue>(_systemContextMock.Object);

            //Act
            await repository.DeleteAsync(expression, It.IsAny<CancellationToken>());

            //Assert
            _systemContextMock.Verify(c => c.GetDbSet<Venue>(), Times.Once);
            mockDbSet.Verify(m => m.RemoveRange(It.IsAny<IEnumerable<Venue>>()), Times.Never);
        }

        [Fact]
        public async Task GetAsync_ShouldCallFirstAsync()
        {
            //Arrange
            var fixture = new Fixture();

            var venues = fixture.Build<Venue>()
                    .Without(v => v.Sections)
                    .Without(v => v.Events)
                    .CreateMany(2);

            var mockDbSet = venues.BuildMockDbSet();
            _systemContextMock.Setup(c => c.GetDbSet<Venue>()).Returns(mockDbSet.Object);
            Expression<Func<Venue, bool>> expression = binding => binding.Id == venues.First().Id;

            var repository = new BaseRepository<Venue>(_systemContextMock.Object);

            //Act
            var venue = await repository.GetAsync(expression, It.IsAny<CancellationToken>());

            //Assert
            _systemContextMock.Verify(c => c.GetDbSet<Venue>(), Times.Once);
            Assert.Equal(venues.First().Id, venue.Id);
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnListAsync()
        {
            //Arrange
            var fixture = new Fixture();

            var venues = fixture.Build<Venue>()
                    .Without(v => v.Sections)
                    .Without(v => v.Events)
                    .CreateMany(2);

            var mockDbSet = venues.BuildMockDbSet();
            _systemContextMock.Setup(c => c.GetDbSet<Venue>()).Returns(mockDbSet.Object);

            var repository = new BaseRepository<Venue>(_systemContextMock.Object);

            //Act
            var venuesDb = await repository.GetListAsync(It.IsAny<CancellationToken>());

            //Assert
            _systemContextMock.Verify(c => c.GetDbSet<Venue>(), Times.Once);
            Assert.Equal(venues.Select(v => v.Id), venuesDb.Select(v => v.Id));
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnList_OneElementByConditionAsync()
        {
            //Arrange
            var fixture = new Fixture();

            var venues = fixture.Build<Venue>()
                    .Without(v => v.Sections)
                    .Without(v => v.Events)
                    .CreateMany(2);

            var mockDbSet = venues.BuildMockDbSet();
            _systemContextMock.Setup(c => c.GetDbSet<Venue>()).Returns(mockDbSet.Object);
            Expression<Func<Venue, bool>> expression = binding => binding.Id == venues.First().Id;

            var repository = new BaseRepository<Venue>(_systemContextMock.Object);

            //Act
            var venuesDb = await repository.GetListAsync(expression, It.IsAny<CancellationToken>());

            //Assert
            _systemContextMock.Verify(c => c.GetDbSet<Venue>(), Times.Once);
            Assert.Equal(venues.First().Id, venuesDb.First().Id);
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnEmptyList_ByConditionAsync()
        {
            //Arrange
            var fixture = new Fixture();

            var venues = fixture.Build<Venue>()
                    .Without(v => v.Sections)
                    .Without(v => v.Events)
                    .CreateMany(2);

            var mockDbSet = venues.BuildMockDbSet();
            _systemContextMock.Setup(c => c.GetDbSet<Venue>()).Returns(mockDbSet.Object);
            Expression<Func<Venue, bool>> expression = binding => false;

            var repository = new BaseRepository<Venue>(_systemContextMock.Object);

            //Act
            var venuesDb = await repository.GetListAsync(expression, It.IsAny<CancellationToken>());

            //Assert
            _systemContextMock.Verify(c => c.GetDbSet<Venue>(), Times.Once);
            Assert.Empty(venuesDb);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateValuesAsync()
        {
            //Arrange
            var fixture = new Fixture();

            var venues = fixture.Build<Venue>()
                    .Without(v => v.Sections)
                    .Without(v => v.Events)
                    .CreateMany(2);

            var mockDbSet = venues.BuildMockDbSet();

            #region entry mock

            var stateManagerMock = new Mock<IStateManager>();
            stateManagerMock
                .Setup(x => x.CreateEntityFinder(It.IsAny<IEntityType>()))
                .Returns(new Mock<IEntityFinder>().Object);
            stateManagerMock
                .Setup(x => x.ValueGenerationManager)
                .Returns(new Mock<IValueGenerationManager>().Object);
            stateManagerMock
                .Setup(x => x.InternalEntityEntryNotifier)
                .Returns(new Mock<IInternalEntityEntryNotifier>().Object);

            var entityTypeMock = new Mock<IRuntimeEntityType>();
            var keyMock = new Mock<IKey>();
            keyMock
                .Setup(x => x.Properties)
                .Returns([]);
            entityTypeMock
                .Setup(x => x.FindPrimaryKey())
                .Returns(keyMock.Object);
            entityTypeMock
                .Setup(e => e.EmptyShadowValuesFactory)
                .Returns(() => new Mock<ISnapshot>().Object);

            var internalEntityEntry = new InternalEntityEntry(stateManagerMock.Object, entityTypeMock.Object, venues.First());

            var entityEntryMock = new Mock<EntityEntry<Venue>>(internalEntityEntry);

            var propertyValuesMock = new Mock<PropertyValues>(internalEntityEntry);

            entityEntryMock.Setup(m => m.CurrentValues).Returns(propertyValuesMock.Object);

            #endregion

            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(venues.First());
            mockDbSet.Setup(m => m.Entry(It.IsAny<Venue>())).Returns(entityEntryMock.Object);
            _systemContextMock.Setup(c => c.GetDbSet<Venue>()).Returns(mockDbSet.Object);

            var repository = new BaseRepository<Venue>(_systemContextMock.Object);

            //Act
            await repository.UpdateAsync(venues.First(), It.IsAny<CancellationToken>());

            //Assert
            _systemContextMock.Verify(c => c.GetDbSet<Venue>(), Times.Exactly(2));
            mockDbSet.Verify(m => m.FindAsync(It.IsAny<object[]>()), Times.Once);
            mockDbSet.Verify(m => m.Entry(It.IsAny<Venue>()).CurrentValues.SetValues(It.IsAny<Venue>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotUpdateValues_WhentEntryNotFoundAsync()
        {
            //Arrange
            var fixture = new Fixture();

            var venues = fixture.Build<Venue>()
                    .Without(v => v.Sections)
                    .Without(v => v.Events)
                    .CreateMany(2);

            var mockDbSet = venues.BuildMockDbSet();

            mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Venue?)null);
            _systemContextMock.Setup(c => c.GetDbSet<Venue>()).Returns(mockDbSet.Object);

            var repository = new BaseRepository<Venue>(_systemContextMock.Object);

            //Act
            await repository.UpdateAsync(venues.First(), It.IsAny<CancellationToken>());

            //Assert
            _systemContextMock.Verify(c => c.GetDbSet<Venue>(), Times.Once);
            mockDbSet.Verify(m => m.FindAsync(It.IsAny<object[]>()), Times.Once);
            mockDbSet.Verify(m => m.Entry(It.IsAny<Venue>()).CurrentValues.SetValues(It.IsAny<Venue>()), Times.Never);
        }
    }
}
