using AutoMapper;
using EPAM.EF.Entities;
using EPAM.EF.Entities.Enums.Notifications;
using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.Services.Abstraction;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EPAM.Services
{
    public sealed class NotificationService : BaseService<NotificationService>, INotificationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork">I used MSQL, but Cache can be used too</param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<NotificationService> logger) : base(unitOfWork, mapper, logger)
        {
        }

        public async Task NotifySeatsBooked(Guid cartId, CancellationToken cancellationToken = default)
        {
            //Some values that can be stored inside DB
            const string name = "MyName";
            const string email = "email@g.com";
            const string phone = "+phone";

            var bookedSeats = await UnitOfWork.OrderRepository.GetBookedSeatsAsync(cartId, cancellationToken).ConfigureAwait(false);
            if (bookedSeats == null || bookedSeats.Count == 0) return;

            var content = JsonConvert.SerializeObject(bookedSeats);
            var notification = new Notification()
            {
                NotificationParams = new List<NotificationParam>
                {
                    new NotificationParam{ Key = "Name", Value = name},
                    new NotificationParam{ Key = "Email", Value = email},
                    new NotificationParam{ Key = "Sms", Value = phone}
                },
                Content = content,
                Type = NotificationType.AllTicketsAddedToCheckout
            };

            await UnitOfWork.NotificationRepository.CreateAsync(notification, cancellationToken).ConfigureAwait(false);
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task NotifySeatBooked(Guid priceOption, CancellationToken cancellationToken = default)
        {
            //Some values that can be stored inside DB
            const string name = "MyName";
            const string email = "email@g.com";
            const string phone = "+phone";

            var bookedSeat = await UnitOfWork.OrderRepository.GetBookedSeatAsync(priceOption, cancellationToken).ConfigureAwait(false);
            if (bookedSeat == null) return;

            var content = JsonConvert.SerializeObject(bookedSeat);
            var notification = new Notification()
            {
                NotificationParams = new List<NotificationParam>
                {
                    new NotificationParam{ Key = "Name", Value = name},
                    new NotificationParam{ Key = "Email", Value = email},
                    new NotificationParam{ Key = "Sms", Value = phone}
                },
                Content = content,
                Type = NotificationType.TicketAddedToCheckout
            };

            await UnitOfWork.NotificationRepository.CreateAsync(notification, cancellationToken).ConfigureAwait(false);
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
