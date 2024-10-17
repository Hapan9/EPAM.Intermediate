using EPAM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal sealed class VenueTypeConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_Venues")
                .IsClustered();

            builder
                .Property(p => p.Name)
                .HasMaxLength(100);

        }
    }
}
