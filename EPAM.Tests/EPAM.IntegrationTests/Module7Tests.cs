using EPAM.EF.Entities.Enums;
using EPAM.IntegrationTests.Abstraction;
using EPAM.Services.Dtos.Order;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace EPAM.IntegrationTests
{
    public class Module7Tests : IntegrationTest
    {

        [Fact]
        public async Task Test1000Request()
        {
            //Arrange
            var priceOption = await App.Context.PriceOptions.FirstAsync();
            var order = new CreateOrderDto
            {
                EventId = priceOption.EventId,
                SeatId = priceOption.SeatId,
                PriceOptionId = priceOption.Id
            };
            var json = JsonConvert.SerializeObject(order);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var statuses = new List<HttpStatusCode>();

            //Act
            var lockObject = new object();
            var tasks = new List<Task>();

            for (var i = 0; i < 1000; i++)
            {
                var task = Task.Run(async () =>
                {
                    var result = await App.Client.PostAsync($"api/Orders/carts/{Guid.NewGuid()}", httpContent);
                    lock (lockObject)
                    {
                        statuses.Add(result.StatusCode);
                    }
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            var seatStatusUpdated = await App.Context.SeatsStatuses
                .FirstAsync(s => s.SeatId == priceOption.SeatId && s.EventId == priceOption.EventId);
            await App.Context.SeatsStatuses.Entry(seatStatusUpdated).ReloadAsync();

            //Assert
            Assert.Equal(1000, statuses.Count);
            Assert.Single(statuses, HttpStatusCode.OK);
            Assert.Equal(SeatStatus.Booked, seatStatusUpdated.Status);
            Assert.Equal(999, statuses.Where(s => s == HttpStatusCode.BadRequest).Count());
        }
    }
}
