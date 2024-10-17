using EPAM.EF.Entities;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SystemContext).Assembly);
        }
    }
}
