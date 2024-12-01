using AutoFixture;
using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.FakeData
{
    internal static class SeatFakes
    {
        public static IEnumerable<Seat> GenerateSeats(this ModelBuilder modelBuilder, IEnumerable<Raw> raws, int count = 2)
        {
            var seats = new List<Seat>();
            var fixture = new Fixture();
            for (int i = 0; i < count; i++)
            {
                var raw = raws.OrderBy(s => Guid.NewGuid()).First();

                var seat = fixture
                    .Build<Seat>()
                    .Without(s => s.Raw)
                    .Without(s => s.SeatStatuses)
                    .Without(s => s.PriceOptions)
                    .Without(e => e.Orders)
                    .With(s => s.RawId, raw.Id)
                    .Create();

                seats.Add(seat);
            }

            modelBuilder
                .Entity<Seat>()
                .HasData(seats);

            return seats;
        }
    }
}
