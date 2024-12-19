using EPAM.EF.Entities;
using EPAM.EF.Entities.Enums.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPAM.EF.EntityTypeConfigurations
{
    internal sealed class NotificationTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder
                .HasKey(p => p.Id)
                .HasName("PK_Notifications")
                .IsClustered();

            builder
                .Property(n => n.Created)
                .HasDefaultValue(DateTime.Now);

            builder
                .Property(n => n.LastTimeUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValue(DateTime.Now)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

            builder
                .Property(p => p.Type)
                .HasDefaultValue(NotificationType.NotSet);

            builder
                .Property(p => p.Status)
                .HasDefaultValue(NotificationStatus.New);

            builder
                .Property(n => n.Content)
                .HasMaxLength(4000);
        }
    }
}
