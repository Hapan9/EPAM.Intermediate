using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.Consumers
{
    public class SmsConsumer
    {
        private readonly ILoggerFactory _loggerFactory;

        public SmsConsumer(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public async Task Consume(IConnection connection, CancellationToken cancellationToken = default)
        {
            using var channel = await connection.CreateChannelAsync(null, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            var service = new FakeConsumerService(_loggerFactory.CreateLogger<FakeConsumerService>());

            consumer.ReceivedAsync += async (sender, @event) =>
            {
                try
                {
                    var body = @event.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    await service.SendEmail(message);
                    await channel.BasicAckAsync(@event.DeliveryTag, false);
                }
                catch
                {
                    await channel.BasicNackAsync(@event.DeliveryTag, false, false);
                }
            };

            await channel.BasicConsumeAsync("Notification.Sms", false, consumer, cancellationToken);

            while (true) { }
        }
    }
}
