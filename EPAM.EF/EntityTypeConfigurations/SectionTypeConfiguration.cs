using EPAM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal sealed class SectionTypeConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_Sections")
                .IsClustered();

            builder
                .HasOne(p => p.Venue)
                .WithMany(v => v.Sections)
                .HasForeignKey(p => p.VenueId)
                .HasConstraintName("FK_Venues_Sections");

            builder
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
