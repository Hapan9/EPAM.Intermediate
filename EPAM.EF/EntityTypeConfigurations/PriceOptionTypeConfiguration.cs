using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal class PriceOptionTypeConfiguration : IEntityTypeConfiguration<PriceOption>
    {
        public void Configure(EntityTypeBuilder<PriceOption> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_PriceOptions")
                .IsClustered();

            builder
                .Property(p => p.Price)
                .HasColumnType("money");

            builder
                .HasOne(p => p.Event)
                .WithMany(e => e.PriceOptions)
                .HasForeignKey(p => p.EventId)
                .HasConstraintName("FK_Events_PriceOptions")
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Seat)
                .WithMany(e => e.PriceOptions)
                .HasForeignKey(p => p.SeatId)
                .HasConstraintName("FK_Seats_PriceOptions")
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasIndex(p => new { p.EventId, p.SeatId })
                .HasName("IX_PriceOptions_EventId_SeatId")
                .IsUnique();
        }
    }
}
