using EPAM.EF.Entities.Abstraction;
using EPAM.EF.Entities.Enums.Notifications;
using System.ComponentModel.DataAnnotations;

namespace EPAM.EF.Entities
{
    public class Notification : Entity
    {
        [ConcurrencyCheck]
        public NotificationStatus Status { get; set; }

        public NotificationType Type { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastTimeUpdated { get; set; }

        public string? Content { get; set; }

        public virtual List<NotificationParam>? NotificationParams { get; set; }
    }
}
