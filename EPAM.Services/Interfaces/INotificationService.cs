namespace EPAM.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifySeatsBooked(Guid cartId, CancellationToken cancellationToken = default);

        Task NotifySeatBooked(Guid priceOption, CancellationToken cancellationToken = default);
    }
}
