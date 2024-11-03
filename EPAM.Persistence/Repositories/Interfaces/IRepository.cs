using System.Linq.Expressions;

namespace EPAM.Persistence.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);

        Task CreateAsync(T entity, CancellationToken cancellationToken);

        Task UpdateAsync(T entity, CancellationToken cancellationToken);

        Task DeleteAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
    }
}
