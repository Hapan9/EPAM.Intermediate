using EPAM.EF.Entities;
using EPAM.EF.Entities.Enums.Notifications;
using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.RabbitMQ.Consumers;
using EPAM.RabbitMQ.Interfaces;
using EPAM.RabbitMQ.Publishers.Strategy;
using EPAM.RabbitMQ.Publishers.Strategy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.BackgroundServices
{
    public class NotificationsReaderService : BackgroundService
    {
        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly IServiceProvider _services;

        public NotificationsReaderService(IRabbitMqClient rabbitMqClient, IServiceProvider services)
        {
            _rabbitMqClient = rabbitMqClient;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = _services.CreateScope();
                using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var notifications = await unitOfWork.NotificationRepository
                    .GetListAsync(n => n.Status == NotificationStatus.New, 25, stoppingToken)
                    .ConfigureAwait(false);

                foreach (var notification in notifications)
                {
                    IStrategy strategy;
                    try
                    {
                        if (notification.Type == NotificationType.TicketAddedToCheckout)
                        {
                            strategy = new TicketAddedToCheckout(notification);
                            notification.Status = NotificationStatus.InProgress;
                        }
                        else if (notification.Type == NotificationType.AllTicketsAddedToCheckout)
                        {
                            strategy = new AllTicketsAddedToCheckout(notification);
                            notification.Status = NotificationStatus.InProgress;
                        }
                        else if (notification.Type == NotificationType.TicketBookingTimeExpired)
                        {
                            strategy = new TicketBookingTimeExpired(notification);
                            notification.Status = NotificationStatus.InProgress;
                        }
                        else
                        {
                            continue;
                        }

                        await _rabbitMqClient.PublishMessage(strategy, stoppingToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        notification.Status = NotificationStatus.Failure;
                        var notificationResult = new NotificationResult
                        {
                            Reason = ex.Message,
                            Status = NotificationResultStatus.Failure
                        };
                        ConsumersResults.AddNotificationResult(notificationResult);
                        continue;
                    }
                }
                await unitOfWork.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
