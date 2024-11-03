using EPAM.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal class SeatStatusTypeConfiguration : IEntityTypeConfiguration<SeatStatus>
    {
        public void Configure(EntityTypeBuilder<SeatStatus> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_SeatsStatuses")
                .IsClustered();

            builder
                .HasOne(p => p.Event)
                .WithMany(e => e.SeatStatuses)
                .HasForeignKey(p => p.EventId)
                .HasConstraintName("FK_Events_SeatsStatuses")
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Seat)
                .WithMany(e => e.SeatStatuses)
                .HasForeignKey(p => p.SeatId)
                .HasConstraintName("FK_Seats_SeatsStatuses")
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasIndex(p => new { p.EventId, p.SeatId })
                .HasName("IX_SeatsStatuses_EventId_SeatId")
                .IsUnique();
        }
    }
}
