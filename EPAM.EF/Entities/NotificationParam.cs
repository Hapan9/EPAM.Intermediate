using EPAM.EF.Entities.Abstraction;

namespace EPAM.EF.Entities
{
    public class NotificationParam : Entity
    {
        public Guid NotificationId { get; set; }
        public Notification? Notification { get; set; }

        public required string Key { get; set; }

        public string? Value { get; set; }
    }
}
