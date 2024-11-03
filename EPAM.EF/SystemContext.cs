using EPAM.EF.FakeData;
using EPAM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF
{
    public class SystemContext : DbContext
    {
        public SystemContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Raw> Raws { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Venue> Venues { get; set; }

        public DbSet<PriceOption> PriceOptions { get; set; }

        public DbSet<SeatStatus> SeatsStatuses { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SystemContext).Assembly);

            var venues = modelBuilder.GenerateVenues();

            var events = modelBuilder.GenerateEvents(venues);

            var sections = modelBuilder.GenerateSections(venues);

            var raws = modelBuilder.GenerateRaws(sections);

            var seats = modelBuilder.GenerateSeats(raws);

            modelBuilder.GeneratePriceOptions(events, seats);

            modelBuilder.GenerateSeatStatus(events, seats);
        }
    }
}
