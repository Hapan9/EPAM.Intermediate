using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal class NotificationParamTypeConfiguration : IEntityTypeConfiguration<NotificationParam>
    {
        public void Configure(EntityTypeBuilder<NotificationParam> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_NotificationsParams")
                .IsClustered();

            builder
                .HasOne(n => n.Notification)
                .WithMany(n => n.NotificationParams)
                .HasForeignKey(n => n.NotificationId)
                .HasConstraintName("FK_Notifications_NotificationsParams");

            builder
                .Property(n => n.Key)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(n => n.Value)
                .HasMaxLength(100);

            builder
                .HasIndex(n => new { n.NotificationId, n.Key })
                .HasName("IX_NotificationsParams_NotificationId_Key")
                .IsUnique();
        }
    }
}
