using EPAM.RabbitMQ.Publishers.Abstraction;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.Publishers
{
    public class UserNotificationPublisher : BasePublisher
    {
        public string _header;
        public const string MainHeader = "Notification";
        public const string SmsHeader = "Sms";
        public const string EmailHeader = "Email";

        public UserNotificationPublisher(string content) : base(content)
        {
            _header = MainHeader;
        }

        public UserNotificationPublisher UseSmsNotification(bool useNotification = true)
        {
            if (useNotification) _header += $".{SmsHeader}";

            return this;
        }

        public UserNotificationPublisher UseEmailNotification(bool useNotification = true)
        {
            if (useNotification) _header += $".{EmailHeader}";

            return this;
        }

        public override async Task PublishMessageAsync(IConnection connection, CancellationToken cancellationToken = default)
        {
            var body = Encoding.UTF8.GetBytes(Content);

            const string NotificationExchange = "Notification.Exchange";
            const string EmailNotificationQueue = "Notification.Email";
            const string SmsNotificationQueue = "Notification.Sms";

            using var channel = await connection.CreateChannelAsync(null, cancellationToken).ConfigureAwait(false);

            await channel.ExchangeDeclareAsync(NotificationExchange, "topic", true, false, null, false, false, cancellationToken).ConfigureAwait(false);
            await channel.QueueDeclareAsync(EmailNotificationQueue, true, false, false, null, false, false, cancellationToken).ConfigureAwait(true);
            await channel.QueueDeclareAsync(SmsNotificationQueue, true, false, false, null, false, false, cancellationToken).ConfigureAwait(true);

            await channel.QueueBindAsync(EmailNotificationQueue, NotificationExchange, $"{MainHeader}.#.{EmailHeader}.#", null, false, cancellationToken).ConfigureAwait(false);
            await channel.QueueBindAsync(SmsNotificationQueue, NotificationExchange, $"{MainHeader}.#.{SmsHeader}.#", null, false, cancellationToken).ConfigureAwait(false);

            await channel.BasicPublishAsync(NotificationExchange, _header, body, cancellationToken).ConfigureAwait(false);
        }
    }
}
