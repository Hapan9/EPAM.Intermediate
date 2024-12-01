using AutoFixture;
using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.FakeData
{
    internal static class SeatStatusFakes
    {
        public static IEnumerable<SeatStatus> GenerateSeatStatus(this ModelBuilder modelBuilder, IEnumerable<Event> events, IEnumerable<Seat> seats)
        {
            var seatStatuses = new List<SeatStatus>();
            var fixture = new Fixture();

            foreach (var eventR in events)
            {
                foreach (var seat in seats)
                {
                    var seatStatus = fixture
                    .Build<SeatStatus>()
                    .Without(o => o.Seat)
                    .Without(o => o.Event)
                    .With(o => o.EventId, eventR.Id)
                    .With(o => o.SeatId, seat.Id)
                    .With(o => o.Status, Entities.Enums.SeatStatus.Available)
                    .Without(o => o.LastStatusChangeDt)
                    .Create();

                    seatStatuses.Add(seatStatus);
                }
            }

            modelBuilder
                .Entity<SeatStatus>()
                .HasData(seatStatuses);

            return seatStatuses;
        }
    }
}
