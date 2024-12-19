using EPAM.EF.Entities;

namespace EPAM.EF.Repositories.Interfaces
{
    public interface INotificationResultRepository : IRepository<NotificationResult>
    {
        Task AddRangeAsync(IEnumerable<NotificationResult> notificationResults, CancellationToken cancellationToken = default);
    }
}
