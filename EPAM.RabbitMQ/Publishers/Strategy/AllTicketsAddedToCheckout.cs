using EPAM.EF.Entities;
using EPAM.EF.Models;
using EPAM.RabbitMQ.Publishers.Interfaces;
using EPAM.RabbitMQ.Publishers.Strategy.Abstraction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPAM.RabbitMQ.Publishers.Strategy
{
    internal class AllTicketsAddedToCheckout : BaseStrategy
    {
        public AllTicketsAddedToCheckout(Notification notification) : base(notification)
        {
        }

        public override IPublisher CreatePublisher()
        {
            var bookedSeats = JsonConvert.DeserializeObject<List<SeatBooked>>(Notification.Content!);
            if (bookedSeats == null || bookedSeats.Count == 0) throw new Exception("Content is empty");

            var message = string.Empty;
            decimal price = 0;
            foreach (var bookedSeat in bookedSeats)
            {
                message += $"You booked seat №{bookedSeat.SeatNumber}, raw №{bookedSeat.RawNumber}, section {bookedSeat.SectionName}, venue {bookedSeat.VenueName}, event {bookedSeat.EventName} \r\n";
                price += bookedSeat.Price;
            }
            message += $"Total price - {price}";


            var title = "Dear Client, thank you";

            if (Notification.NotificationParams == null || Notification.NotificationParams.Count == 0)
            {
                title = $"{title}\r\n{message}";
                return new UserNotificationPublisher(title); ;
            }

            if (Notification.NotificationParams.Exists(p => string.Equals(p.Key, "Name", StringComparison.CurrentCultureIgnoreCase)))
            {
                var name = Notification.NotificationParams.Where(p => string.Equals(p.Key, "Name", StringComparison.CurrentCultureIgnoreCase)).First().Value;
                title = $"Dear {name}, thank you\r\n{message}";
            }
            else
            {
                title = $"{title}\r\n{message}";
            }

            var publisher = new UserNotificationPublisher(title);

            if (Notification.NotificationParams.Exists(p => string.Equals(p.Key, "sms", StringComparison.CurrentCultureIgnoreCase)))
            {
                publisher.UseSmsNotification();
            }

            if (Notification.NotificationParams.Exists(p => string.Equals(p.Key, "email", StringComparison.CurrentCultureIgnoreCase)))
            {
                publisher.UseEmailNotification();
            }

            return publisher;
        }
    }
}
