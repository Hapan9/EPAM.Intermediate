using EPAM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal sealed class EventTypeConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_Events")
                .IsClustered();

            builder
                .HasOne(p => p.Venue)
                .WithMany(v => v.Events)
                .HasForeignKey(p => p.VenueId)
                .HasConstraintName("FK_Venues_Events");

            builder
                .Property(p => p.Name)
                .HasMaxLength(100);
        }
    }
}
