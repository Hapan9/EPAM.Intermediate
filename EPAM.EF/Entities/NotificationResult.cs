using EPAM.EF.Entities.Abstraction;
using EPAM.EF.Entities.Enums.Notifications;

namespace EPAM.EF.Entities
{
    public class NotificationResult : Entity
    {
        public NotificationResultStatus Status { get; set; }

        public string? Content { get; set; }

        public string? Reason { get; set; }

        public Guid? NotificationId { get; set; }
        public virtual Notification? Notification { get; set; }
    }
}
