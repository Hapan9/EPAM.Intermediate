namespace EPAM.RabbitMQ.Constants
{
    internal static class RabbitMqConstants
    {
        public const string NotificationExchange = "Notification.Exchange";
        public const string NotificationRetryExchange = "Notification.Retry.Exchange";

        public const string EmailNotificationQueue = "Notification.Email";
        public const string SmsNotificationQueue = "Notification.Sms";
        public const string RetryNotificationQueue = "Notification.Retry";
    }
}
