using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.RabbitMQ.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.BackgroundServices
{
    public class ConsumersResultsService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public ConsumersResultsService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var notificationsResults = ConsumersResults.GetResults();
                if (notificationsResults.Count() == 0) continue;

                using var scope = _services.CreateScope();
                using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                await unitOfWork.NotificationResultRepository.AddRangeAsync(notificationsResults, stoppingToken).ConfigureAwait(false);
                await unitOfWork.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
