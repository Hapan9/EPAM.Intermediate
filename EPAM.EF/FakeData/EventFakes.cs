using AutoFixture;
using EPAM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.FakeData
{
    public static class EventFakes
    {
        public static IEnumerable<Event> GenerateEvents(this ModelBuilder modelBuilder, IEnumerable<Venue> venues, int count = 1)
        {
            var events = new List<Event>();
            var fixture = new Fixture();
            for (int i = 0; i < count; i++)
            {
                var venue = venues.OrderBy(v => Guid.NewGuid()).First();

                var eventR = fixture
                    .Build<Event>()
                    .Without(e => e.SeatStatuses)
                    .Without(e => e.PriceOptions)
                    .Without(e => e.Orders)
                    .Without(e => e.Venue)
                    .With(e => e.VenueId, venue.Id)
                    .Create();

                events.Add(eventR);
            }

            modelBuilder
                .Entity<Event>()
                .HasData(events);

            return events;
        }
    }
}
