using AutoFixture;
using EPAM.EF.Entities;
using EPAM.IntegrationTests.Abstraction;
using EPAM.Services.Dtos.Order;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace EPAM.IntegrationTests
{
    public sealed class OrdersControllerTests : IntegrationTest
    {
        Fixture _fixture;

        public OrdersControllerTests() : base()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnOrderAsync()
        {
            //Arrange
            var priceOptions = await App.Context.PriceOptions.FirstAsync();
            var order = _fixture.Build<Order>()
                .With(o => o.EventId, priceOptions.EventId)
                .With(o => o.PriceOptionId, priceOptions.Id)
                .With(o => o.SeatId, priceOptions.SeatId)
                .Without(o => o.Seat)
                .Without(o => o.Event)
                .Without(o => o.PriceOption)
                .Without(o => o.Payment)
                .Without(o => o.PaymentId)
                .Create();
            await App.Context.Orders.AddAsync(order);
            await App.Context.SaveChangesAsync();

            //Act
            var response = await App.Client.GetAsync($"api/Orders/carts/{order.CartId}");
            var responseMessage = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<List<GetOrderDto>>(responseMessage);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseObject);
            Assert.Contains(order.CartId, responseObject.Select(o => o.CartId));
        }

        [Fact]
        public async Task PostAsync_ShouldCreateNewOrderAsync()
        {
            //Arrange
            var cartId = _fixture.Create<Guid>();
            var priceOptions = await App.Context.PriceOptions.FirstAsync();

            var createOrderDto = new CreateOrderDto
            {
                EventId = priceOptions.EventId,
                PriceOptionId = priceOptions.Id,
                SeatId = priceOptions.SeatId
            };
            var json = JsonConvert.SerializeObject(createOrderDto);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await App.Client.PostAsync($"api/Orders/carts/{cartId}", httpContent);
            var responseMessage = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<List<GetOrderDto>>(responseMessage);
            var order = await App.Context.Orders.FirstOrDefaultAsync(o => o.CartId == cartId);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(order);
            Assert.NotNull(responseObject);
            Assert.Contains(order.CartId, responseObject.Select(o => o.CartId));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteOrderAsync()
        {
            //Arrange
            var priceOptions = await App.Context.PriceOptions.FirstAsync();
            var order = _fixture.Build<Order>()
                .With(o => o.EventId, priceOptions.EventId)
                .With(o => o.PriceOptionId, priceOptions.Id)
                .With(o => o.SeatId, priceOptions.SeatId)
                .Without(o => o.Seat)
                .Without(o => o.Event)
                .Without(o => o.PriceOption)
                .Without(o => o.Payment)
                .Without(o => o.PaymentId)
                .Create();
            await App.Context.Orders.AddAsync(order);
            await App.Context.SaveChangesAsync();

            //Act
            var response = await App.Client.DeleteAsync($"api/Orders/carts/{order.CartId}/events/{order.EventId}/seats/{order.SeatId}");
            var orderUpdated = await App.Context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Null(orderUpdated);
        }

        [Fact]
        public async Task PutAsync_BookSeatsForCartAsync()
        {
            //Arrange
            var priceOption = await App.Context.PriceOptions.FirstAsync();
            var order = _fixture.Build<Order>()
                .With(o => o.EventId, priceOption.EventId)
                .With(o => o.PriceOptionId, priceOption.Id)
                .With(o => o.SeatId, priceOption.SeatId)
                .Without(o => o.Seat)
                .Without(o => o.Event)
                .Without(o => o.PriceOption)
                .Without(o => o.Payment)
                .Without(o => o.PaymentId)
                .Create();
            await App.Context.Orders.AddAsync(order);
            await App.Context.SaveChangesAsync();

            //Act
            var response = await App.Client.PutAsync($"api/Orders/carts/{order.CartId}/book", null);
            var responseMessage = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Guid>(responseMessage);
            var orderUpdated = await App.Context.Orders.FirstAsync(o => o.Id == order.Id);
            var seatStatusUpdated = await App.Context.SeatsStatuses.FirstAsync(s => s.SeatId == order.SeatId);
            var payments = await App.Context.Payments.ToListAsync();

            await App.Context.Orders.Entry(orderUpdated).ReloadAsync();
            await App.Context.SeatsStatuses.Entry(seatStatusUpdated).ReloadAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(string.IsNullOrEmpty(responseMessage));
            Assert.NotEmpty(payments);
            Assert.Equal(EF.Entities.Enums.SeatStatus.Booked, seatStatusUpdated.Status);
            Assert.Equal(orderUpdated.PaymentId, responseObject);
        }
    }
}
