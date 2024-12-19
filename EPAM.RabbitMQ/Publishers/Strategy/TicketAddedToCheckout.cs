using EPAM.EF.Entities;
using EPAM.EF.Models;
using EPAM.RabbitMQ.Publishers.Interfaces;
using EPAM.RabbitMQ.Publishers.Strategy.Abstraction;
using Newtonsoft.Json;
using System;

namespace EPAM.RabbitMQ.Publishers.Strategy
{
    internal sealed class TicketAddedToCheckout : BaseStrategy
    {
        public TicketAddedToCheckout(Notification notification) : base(notification)
        {
        }

        public override IPublisher CreatePublisher()
        {
            var bookedSeat = JsonConvert.DeserializeObject<SeatBooked>(Notification.Content!);
            if (bookedSeat == null) throw new Exception("Content is empty");

            var message = $"You booked seat №{bookedSeat.SeatNumber}, raw №{bookedSeat.RawNumber}, section {bookedSeat.SectionName}, venue {bookedSeat.VenueName}, event {bookedSeat.EventName} \r\nPrice - {bookedSeat.Price}";

            var publisher = new UserNotificationPublisher(Notification, message);

            if (Notification.NotificationParams == null || Notification.NotificationParams.Count == 0) return publisher;

            if (Notification.NotificationParams.Exists(p => string.Equals(p.Key, "email", StringComparison.CurrentCultureIgnoreCase)))
            {
                publisher.UseEmailNotification();
            }

            return publisher;
        }
    }
}
