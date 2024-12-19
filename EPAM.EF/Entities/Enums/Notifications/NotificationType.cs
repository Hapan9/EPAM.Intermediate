using System.ComponentModel;

namespace EPAM.EF.Entities.Enums.Notifications
{
    public enum NotificationType
    {
        NotSet = 0,
        [Description("Ticket added to checkout")]
        TicketAddedToCheckout = 1,
        [Description("All tickets have been added to checkout")]
        AllTicketsAddedToCheckout = 2,
        [Description("Ticket booking time has expired")]
        TicketBookingTimeExpired = 3
    }
}
