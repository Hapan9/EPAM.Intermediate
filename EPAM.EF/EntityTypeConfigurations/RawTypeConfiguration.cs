using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal sealed class RawTypeConfiguration : IEntityTypeConfiguration<Raw>
    {
        public void Configure(EntityTypeBuilder<Raw> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_Raws")
                .IsClustered();

            builder
                .HasOne(p => p.Section)
                .WithMany(s => s.Raws)
                .HasForeignKey(p => p.SectionId)
                .HasConstraintName("FK_Sections_Raws");

        }
    }
}
