using EPAM.EF.Entities;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetListAsync(Expression<Func<Notification, bool>> expression, int count = 100, CancellationToken cancellationToken = default);
    }
}
