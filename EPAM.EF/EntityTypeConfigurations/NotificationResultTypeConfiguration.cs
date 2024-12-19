using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal class NotificationResultTypeConfiguration : IEntityTypeConfiguration<NotificationResult>
    {
        public void Configure(EntityTypeBuilder<NotificationResult> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_NotificationsResults")
                .IsClustered();

            builder
                .HasOne(n => n.Notification)
                .WithMany(n => n.NotificationResults)
                .HasForeignKey(n => n.NotificationId)
                .HasConstraintName("FK_Notifications_NotificationsResults");

            builder
                .Property(p => p.Content)
                .HasMaxLength(1000);

            builder
                .Property(p => p.Reason)
                .HasMaxLength(1000);
        }
    }
}
