using AutoFixture;
using EPAM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.FakeData
{
    public static class RawsFakes
    {
        public static IEnumerable<Raw> GenerateRaws(this ModelBuilder modelBuilder, IEnumerable<Section> sections, int count = 2)
        {
            var raws = new List<Raw>();
            var fixture = new Fixture();
            for (int i = 0; i < count; i++)
            {
                var section = sections.OrderBy(s => Guid.NewGuid()).First();

                var raw = fixture
                    .Build<Raw>()
                    .Without(r => r.Section)
                    .Without(r => r.Seats)
                    .With(r => r.SectionId, section.Id)
                    .Create();

                raws.Add(raw);
            }

            modelBuilder
                .Entity<Raw>()
                .HasData(raws);

            return raws;
        }
    }
}
