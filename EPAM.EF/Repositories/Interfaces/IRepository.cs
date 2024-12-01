using System.Linq.Expressions;

namespace EPAM.EF.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        Task CreateAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    }
}
