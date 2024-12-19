using EPAM.EF.Entities;
using EPAM.EF.Interfaces;
using EPAM.EF.Repositories.Abstraction;
using EPAM.EF.Repositories.Interfaces;

namespace EPAM.EF.Repositories
{
    public sealed class NotificationResultRepository : BaseRepository<NotificationResult>, INotificationResultRepository
    {
        public NotificationResultRepository(ISystemContext context) : base(context)
        {
        }

        public async Task AddRangeAsync(IEnumerable<NotificationResult> notificationResults, CancellationToken cancellationToken = default)
        {
            await Context.NotificationsResults.AddRangeAsync(notificationResults, cancellationToken).ConfigureAwait(false);
        }
    }
}
