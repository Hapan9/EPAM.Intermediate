using AutoFixture;
using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.FakeData
{
    public static class VenueFakes
    {
        public static IEnumerable<Venue> GenerateVenues(this ModelBuilder modelBuilder, int count = 3)
        {
            var venues = new Fixture()
                .Build<Venue>()
                .Without(v => v.Sections)
                .Without(v => v.Events)
                .CreateMany(count);

            modelBuilder
                .Entity<Venue>()
                .HasData(venues);

            return venues;
        }
    }
}
