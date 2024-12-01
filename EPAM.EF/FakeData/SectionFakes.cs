using AutoFixture;
using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.FakeData
{
    internal static class SectionFakes
    {
        public static IEnumerable<Section> GenerateSections(this ModelBuilder modelBuilder, IEnumerable<Venue> venues, int count = 2)
        {
            var sections = new List<Section>();
            var fixture = new Fixture();
            for (int i = 0; i < count; i++)
            {
                var venue = venues.OrderBy(v => Guid.NewGuid()).First();

                var section = fixture
                    .Build<Section>()
                    .Without(s => s.Raws)
                    .Without(s => s.Venue)
                    .With(s => s.VenueId, venue.Id)
                    .Create();

                sections.Add(section);
            }

            modelBuilder
                .Entity<Section>()
                .HasData(sections);

            return sections;
        }
    }
}
