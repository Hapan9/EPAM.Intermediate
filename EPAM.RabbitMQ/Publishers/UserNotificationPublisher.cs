using EPAM.EF.Entities;
using EPAM.RabbitMQ.Constants;
using EPAM.RabbitMQ.Publishers.Abstraction;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.Publishers
{
    public class UserNotificationPublisher : BasePublisher
    {
        public string _header;
        private readonly Notification notification;

        public UserNotificationPublisher(Notification notification, string content) : base(content)
        {
            _header = UserNotificationConstants.MainHeader;
            this.notification = notification;
        }

        public UserNotificationPublisher UseSmsNotification(bool useNotification = true)
        {
            if (useNotification) _header += $".{UserNotificationConstants.SmsHeader}";

            return this;
        }

        public UserNotificationPublisher UseEmailNotification(bool useNotification = true)
        {
            if (useNotification) _header += $".{UserNotificationConstants.EmailHeader}";

            return this;
        }

        public override async Task PublishMessageAsync(IConnection connection, CancellationToken cancellationToken = default)
        {
            var body = Encoding.UTF8.GetBytes(Content);

            using var channel = await connection.CreateChannelAsync(null, cancellationToken).ConfigureAwait(false);

            var basicProperties = new BasicProperties();
            basicProperties.Headers = new Dictionary<string, object?>
            {
                { "NotoficationId", notification.Id.ToString() }
            };
            await channel.BasicPublishAsync(RabbitMqConstants.NotificationExchange, _header, false, basicProperties, body, cancellationToken).ConfigureAwait(false);
        }
    }
}
