using EPAM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal sealed class SeatTypeConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_Seats")
                .IsClustered();

            builder
                .HasOne(p => p.Raw)
                .WithMany(r => r.Seats)
                .HasForeignKey(p => p.RawId)
                .HasConstraintName("FK_Raws_Seats");
        }
    }
}
