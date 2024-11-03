using AutoFixture;
using EPAM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.FakeData
{
    internal static class PriceOptionFakes
    {
        public static IEnumerable<PriceOption> GeneratePriceOptions(this ModelBuilder modelBuilder, IEnumerable<Event> events, IEnumerable<Seat> seats, int count = 20)
        {
            var priceOptions = new List<PriceOption>();
            var fixture = new Fixture();

            foreach (var eventR in events)
            {
                foreach (var seat in seats)
                {
                    var priceOption = fixture
                    .Build<PriceOption>()
                    .Without(o => o.Seat)
                    .Without(o => o.Event)
                    .Without(o => o.Order)
                    .With(o => o.EventId, eventR.Id)
                    .With(o => o.SeatId, seat.Id)
                    .Create();

                    priceOptions.Add(priceOption);
                }
            }

            modelBuilder
                .Entity<PriceOption>()
                .HasData(priceOptions);

            return priceOptions;
        }
    }
}
