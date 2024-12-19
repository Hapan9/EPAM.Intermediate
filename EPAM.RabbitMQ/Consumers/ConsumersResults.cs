using EPAM.EF.Entities;
using System.Collections.Generic;

namespace EPAM.RabbitMQ.Consumers
{
    public static class ConsumersResults
    {
        private static List<NotificationResult> _notificationsResults = new List<NotificationResult>();
        private static object lockObject = new object();

        public static void AddNotificationResult(NotificationResult notificationResult)
        {
            lock (lockObject)
            {
                _notificationsResults.Add(notificationResult);
            }
        }

        public static IEnumerable<NotificationResult> GetResults()
        {
            lock (lockObject)
            {
                var result = _notificationsResults.ToArray();
                _notificationsResults.Clear();

                return result;
            }
        }
    }
}
