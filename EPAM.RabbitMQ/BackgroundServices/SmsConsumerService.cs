using EPAM.EF.Entities;
using EPAM.EF.Entities.Enums.Notifications;
using EPAM.RabbitMQ.Constants;
using EPAM.RabbitMQ.Consumers;
using EPAM.RabbitMQ.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.BackgroundServices
{
    public class SmsConsumerService : BackgroundService
    {
        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceProvider _services;

        public SmsConsumerService(IRabbitMqClient rabbitMqClient, ILoggerFactory loggerFactory, IServiceProvider services)
        {
            _rabbitMqClient = rabbitMqClient;
            _loggerFactory = loggerFactory;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _rabbitMqClient.GetConnection();
            using var channel = await connection.CreateChannelAsync(null, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            var service = new FakeConsumerService(_loggerFactory.CreateLogger<FakeConsumerService>());

            consumer.ReceivedAsync += async (sender, @event) =>
            {
                try
                {
                    var body = @event.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Guid? notificationId = null;
                    if (@event.BasicProperties.Headers != null && @event.BasicProperties.Headers.ContainsKey("NotoficationId"))
                    {
                        notificationId = Guid.Parse(Encoding.UTF8.GetString((byte[])@event.BasicProperties.Headers["NotoficationId"]!));
                    }

                    if (@event.BasicProperties.Headers != null && @event.BasicProperties.Headers.ContainsKey("x-death"))
                    {
                        var deathProperties = (List<object>)@event.BasicProperties.Headers["x-death"]!;
                        var lastRetry = (Dictionary<string, object>)deathProperties[0];
                        var count = lastRetry["count"];
                        //* 2 since moving from retry queue to main one also incereces counter
                        if ((long)count > 3 * 2)
                        {
                            await channel.BasicAckAsync(@event.DeliveryTag, false);
                            var failedNotificationResult = new NotificationResult { Status = NotificationResultStatus.Failure, NotificationId = notificationId, Content = message, Reason = "Too many tries" };
                            ConsumersResults.AddNotificationResult(failedNotificationResult);
                            return;
                        }
                    }


                    if (string.Equals("test", message, StringComparison.CurrentCultureIgnoreCase))
                    {
                        await channel.BasicRejectAsync(@event.DeliveryTag, false, stoppingToken).ConfigureAwait(false);
                        return;
                    }

                    await service.SendSms(message);

                    var notificationResult = new NotificationResult { Status = NotificationResultStatus.Success, NotificationId = notificationId };
                    ConsumersResults.AddNotificationResult(notificationResult);
                    await channel.BasicAckAsync(@event.DeliveryTag, false);
                }
                catch
                {
                    await channel.BasicRejectAsync(@event.DeliveryTag, false, stoppingToken).ConfigureAwait(false);
                }
            };

            await channel.BasicConsumeAsync(RabbitMqConstants.SmsNotificationQueue, false, consumer, stoppingToken);

            while (!stoppingToken.IsCancellationRequested) { }
        }
    }
}
