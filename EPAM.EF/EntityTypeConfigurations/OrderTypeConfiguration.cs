using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal sealed class OrderTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_Orders")
                .IsClustered();

            builder
                .HasOne(p => p.Event)
                .WithMany(e => e.Orders)
                .HasForeignKey(p => p.EventId)
                .HasConstraintName("FK_Events_Orders")
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Seat)
                .WithMany(e => e.Orders)
                .HasForeignKey(p => p.SeatId)
                .HasConstraintName("FK_Seats_Orders")
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.PriceOption)
                .WithOne(o => o.Order)
                .HasForeignKey<Order>(p => p.PriceOptionId)
                .HasConstraintName("FK_PriceOptions_Orders")
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Payment)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.PaymentId)
                .HasConstraintName("FK_Payments_Orders")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasIndex(p => new { p.EventId, p.SeatId, p.PriceOptionId })
                .HasName("IX_Orders_EventId_SeatId_PriceOptionId")
                .IsUnique();
        }
    }
}
