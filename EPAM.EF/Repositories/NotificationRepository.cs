using EPAM.EF.Entities;
using EPAM.EF.Interfaces;
using EPAM.EF.Repositories.Abstraction;
using EPAM.EF.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories
{
    public sealed class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ISystemContext context) : base(context)
        {
        }

        public override async Task<Notification> GetAsync(Expression<Func<Notification, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Context.Notifications
                .Include(p => p.NotificationParams)
                .FirstAsync(expression, cancellationToken)
                .ConfigureAwait(false);
        }

        public override async Task<IEnumerable<Notification>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Notifications
                .Include(p => p.NotificationParams)
                .OrderBy(n => n.Created)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public override async Task<IEnumerable<Notification>> GetListAsync(Expression<Func<Notification, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Context.Notifications
                .Include(p => p.NotificationParams)
                .Where(expression)
                .OrderBy(n => n.Created)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Notification>> GetListAsync(Expression<Func<Notification, bool>> expression, int count = 100, CancellationToken cancellationToken = default)
        {
            return await Context.Notifications
                .Include(n => n.NotificationParams)
                .Where(expression)
                .OrderBy(n => n.Created)
                .Take(count)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
