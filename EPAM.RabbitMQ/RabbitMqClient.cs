using EPAM.RabbitMQ.Constants;
using EPAM.RabbitMQ.Interfaces;
using EPAM.RabbitMQ.Publishers.Strategy.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ
{
    public class RabbitMqClient : IRabbitMqClient
    {
        ConnectionFactory _factory;
        IConnection? _connection;

        public RabbitMqClient(IConfiguration configuration)
        {
            _factory = configuration.GetSection("RabbitMQ").Get<ConnectionFactory>()!;
        }

        public async Task<IConnection> GetConnection(CancellationToken cancellationToken = default)
        {
            if (_connection != null) return _connection;

            var connection = await _factory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);

            #region RabbitMQ setup

            using var channel = await connection.CreateChannelAsync(null, cancellationToken).ConfigureAwait(false);

            await channel.ExchangeDeclareAsync(RabbitMqConstants.NotificationExchange, "topic", true, false, null, false, false, cancellationToken).ConfigureAwait(false);
            await channel.ExchangeDeclareAsync(RabbitMqConstants.NotificationRetryExchange, "fanout", true, false, null, false, false, cancellationToken).ConfigureAwait(false);

            var emailToDeadLetterQueueSetup = new Dictionary<string, object?>
            {
                {"x-dead-letter-exchange", RabbitMqConstants.NotificationRetryExchange },
                {"x-dead-letter-routing-key", "Notification.Email" }
            };
            await channel.QueueDeclareAsync(RabbitMqConstants.EmailNotificationQueue, true, false, false, emailToDeadLetterQueueSetup, false, false, cancellationToken).ConfigureAwait(true);

            var smsToDeadLetterQueueSetup = new Dictionary<string, object?>
            {
                {"x-dead-letter-exchange", RabbitMqConstants.NotificationRetryExchange },
                {"x-dead-letter-routing-key", "Notification.Sms" }
            };
            await channel.QueueDeclareAsync(RabbitMqConstants.SmsNotificationQueue, true, false, false, smsToDeadLetterQueueSetup, false, false, cancellationToken).ConfigureAwait(true);

            var fromDeadLetterToQueueSetup = new Dictionary<string, object?>
            {
                {"x-dead-letter-exchange", RabbitMqConstants.NotificationExchange },
                {"x-message-ttl", 5000 }
            };
            await channel.QueueDeclareAsync(RabbitMqConstants.RetryNotificationQueue, true, false, false, fromDeadLetterToQueueSetup, false, false, cancellationToken).ConfigureAwait(true);

            await channel.QueueBindAsync(RabbitMqConstants.EmailNotificationQueue, RabbitMqConstants.NotificationExchange, $"{UserNotificationConstants.MainHeader}.#.{UserNotificationConstants.EmailHeader}.#", null, false, cancellationToken).ConfigureAwait(false);
            await channel.QueueBindAsync(RabbitMqConstants.SmsNotificationQueue, RabbitMqConstants.NotificationExchange, $"{UserNotificationConstants.MainHeader}.#.{UserNotificationConstants.SmsHeader}.#", null, false, cancellationToken).ConfigureAwait(false);
            await channel.QueueBindAsync(RabbitMqConstants.RetryNotificationQueue, RabbitMqConstants.NotificationRetryExchange, string.Empty, null, false, cancellationToken).ConfigureAwait(false);

            await channel.DisposeAsync();
            #endregion

            return connection;
        }

        public async Task PublishMessage(IStrategy strategy, CancellationToken cancellationToken = default)
        {
            var connection = await GetConnection().ConfigureAwait(false);
            var publisher = strategy.CreatePublisher();
            await publisher.PublishMessageAsync(connection, cancellationToken).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
