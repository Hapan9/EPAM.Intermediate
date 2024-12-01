using AutoFixture;
using EPAM.EF.Entities;
using EPAM.IntegrationTests.Abstraction;
using EPAM.Services.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace EPAM.IntegrationTests
{
    public sealed class PaymentsControllerTests : IntegrationTest
    {
        private readonly Fixture _fixture;

        public PaymentsControllerTests() : base()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnPaymentAsync()
        {
            //Arrange
            var payment = _fixture.Build<Payment>()
                .Without(p => p.Orders)
                .Create();
            await App.Context.Payments.AddAsync(payment);

            var priceOptions = await App.Context.PriceOptions.FirstAsync();
            var order = _fixture.Build<Order>()
                .With(o => o.EventId, priceOptions.EventId)
                .With(o => o.PriceOptionId, priceOptions.Id)
                .With(o => o.SeatId, priceOptions.SeatId)
                .Without(o => o.Seat)
                .Without(o => o.Event)
                .Without(o => o.PriceOption)
                .Without(o => o.Payment)
                .With(o => o.PaymentId, payment.Id)
                .Create();
            await App.Context.Orders.AddAsync(order);

            priceOptions = await App.Context.PriceOptions.FirstAsync(p => p.Id != priceOptions.Id);
            order = _fixture.Build<Order>()
                .With(o => o.EventId, priceOptions.EventId)
                .With(o => o.PriceOptionId, priceOptions.Id)
                .With(o => o.SeatId, priceOptions.SeatId)
                .Without(o => o.Seat)
                .Without(o => o.Event)
                .Without(o => o.PriceOption)
                .Without(o => o.Payment)
                .With(o => o.PaymentId, payment.Id)
                .Create();
            await App.Context.Orders.AddAsync(order);

            await App.Context.SaveChangesAsync();

            var price = await App.Context.Orders.Where(p => p.PaymentId! == payment.Id).SumAsync(o => o.PriceOption!.Price);

            //Act
            var response = await App.Client.GetAsync($"api/Payments/{payment.Id}");
            var responseMessage = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<PaymentDto>(responseMessage);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseObject);
            Assert.Multiple(
                () => Assert.Equal(payment.Id, responseObject.Id),
                () => Assert.Equal(payment.Status.ToString(), responseObject.Status),
                () => Assert.Equal(price, responseObject.Price));
        }

        [Fact]
        public async Task PutAsync_StatusToComleted_ShouldUpdateStatusesAsync()
        {
            //Arrange
            var payment = _fixture.Build<Payment>()
                .Without(p => p.Orders)
                .With(p => p.Status, EF.Entities.Enums.PaymentStatus.Pending)
                .Create();
            await App.Context.Payments.AddAsync(payment);

            var priceOptions = await App.Context.PriceOptions.FirstAsync();
            var order = _fixture.Build<Order>()
                .With(o => o.EventId, priceOptions.EventId)
                .With(o => o.PriceOptionId, priceOptions.Id)
                .With(o => o.SeatId, priceOptions.SeatId)
                .Without(o => o.Seat)
                .Without(o => o.Event)
                .Without(o => o.PriceOption)
                .Without(o => o.Payment)
                .With(o => o.PaymentId, payment.Id)
                .Create();
            await App.Context.Orders.AddAsync(order);

            priceOptions = await App.Context.PriceOptions.FirstAsync(p => p.Id != priceOptions.Id);
            order = _fixture.Build<Order>()
                .With(o => o.EventId, priceOptions.EventId)
                .With(o => o.PriceOptionId, priceOptions.Id)
                .With(o => o.SeatId, priceOptions.SeatId)
                .Without(o => o.Seat)
                .Without(o => o.Event)
                .Without(o => o.PriceOption)
                .Without(o => o.Payment)
                .With(o => o.PaymentId, payment.Id)
                .Create();
            await App.Context.Orders.AddAsync(order);

            await App.Context.SaveChangesAsync();

            var price = await App.Context.Orders.Where(p => p.PaymentId! == payment.Id).SumAsync(o => o.PriceOption!.Price);

            //Act
            var response = await App.Client.PutAsync($"api/Payments/{payment.Id}/completed", null);
            var paymentUpdated = await App.Context.Payments.FirstAsync(p => p.Id == payment.Id);
            var seatStatuses = await App.Context.SeatsStatuses.Where(s => s.Seat!.Orders!.Select(o => o.PaymentId).Contains(payment.Id)).Select(s => s.Status).ToListAsync();

            await App.Context.Payments.Entry(paymentUpdated).ReloadAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(EF.Entities.Enums.PaymentStatus.Completed, paymentUpdated.Status);
            Assert.Equal(2, seatStatuses.Count);
            Assert.All(seatStatuses, s => Assert.Equal(EF.Entities.Enums.SeatStatus.Sold, s));
        }

        [Fact]
        public async Task PutAsync_StatusToFailed_ShouldUpdateStatusesAsync()
        {
            //Arrange
            var payment = _fixture.Build<Payment>()
                .Without(p => p.Orders)
                .With(p => p.Status, EF.Entities.Enums.PaymentStatus.Pending)
                .Create();
            await App.Context.Payments.AddAsync(payment);

            var priceOptions = await App.Context.PriceOptions.FirstAsync();
            var order = _fixture.Build<Order>()
                .With(o => o.EventId, priceOptions.EventId)
                .With(o => o.PriceOptionId, priceOptions.Id)
                .With(o => o.SeatId, priceOptions.SeatId)
                .Without(o => o.Seat)
                .Without(o => o.Event)
                .Without(o => o.PriceOption)
                .Without(o => o.Payment)
                .With(o => o.PaymentId, payment.Id)
                .Create();
            await App.Context.Orders.AddAsync(order);

            priceOptions = await App.Context.PriceOptions.FirstAsync(p => p.Id != priceOptions.Id);
            order = _fixture.Build<Order>()
                .With(o => o.EventId, priceOptions.EventId)
                .With(o => o.PriceOptionId, priceOptions.Id)
                .With(o => o.SeatId, priceOptions.SeatId)
                .Without(o => o.Seat)
                .Without(o => o.Event)
                .Without(o => o.PriceOption)
                .Without(o => o.Payment)
                .With(o => o.PaymentId, payment.Id)
                .Create();
            await App.Context.Orders.AddAsync(order);

            await App.Context.SaveChangesAsync();

            var price = await App.Context.Orders.Where(p => p.PaymentId! == payment.Id).SumAsync(o => o.PriceOption!.Price);

            //Act
            var response = await App.Client.PutAsync($"api/Payments/{payment.Id}/failed", null);
            var paymentUpdated = await App.Context.Payments.FirstAsync(p => p.Id == payment.Id);
            var seatStatuses = await App.Context.SeatsStatuses.Where(s => s.Seat!.Orders!.Select(o => o.PaymentId).Contains(payment.Id)).Select(s => s.Status).ToListAsync();

            await App.Context.Payments.Entry(paymentUpdated).ReloadAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(EF.Entities.Enums.PaymentStatus.Declined, paymentUpdated.Status);
            Assert.Equal(2, seatStatuses.Count);
            Assert.All(seatStatuses, s => Assert.Equal(EF.Entities.Enums.SeatStatus.Available, s));
        }
    }
}
