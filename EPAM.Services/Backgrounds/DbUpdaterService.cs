using EPAM.EF.Entities.Enums;
using EPAM.EF.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace EPAM.Services.Backgrounds
{
    public class DbUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public DbUpdaterService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = _services.CreateScope();
                using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                await unitOfWork.BeginTransaction(IsolationLevel.RepeatableRead, stoppingToken).ConfigureAwait(false);

                var bookedSeats = await unitOfWork.SeatStatusRepository
                    .GetListAsync(s => s.Status == SeatStatus.Booked && s.LastStatusChangeDt.HasValue && s.LastStatusChangeDt.Value.AddSeconds(30) < DateTime.Now,
                        stoppingToken)
                    .ConfigureAwait(false);

                foreach (var seat in bookedSeats)
                {
                    seat.Status = SeatStatus.Available;
                    seat.LastStatusChangeDt = DateTime.Now;
                    seat.Version = Guid.NewGuid();
                }

                await unitOfWork.SaveChangesAsync(stoppingToken).ConfigureAwait(false);

                await unitOfWork.CommitTransaction(stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
