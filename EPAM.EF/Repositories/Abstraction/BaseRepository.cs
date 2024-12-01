using EPAM.EF.Entities.Abstraction;
using EPAM.EF.Interfaces;
using EPAM.EF.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories.Abstraction
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly ISystemContext Context;

        public BaseRepository(ISystemContext context)
        {
            Context = context;
        }

        public virtual async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await Context.GetDbSet<T>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            var enteties = await Context.GetDbSet<T>().Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (enteties.Count == 0) return;

            Context.GetDbSet<T>().RemoveRange(enteties);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Context.GetDbSet<T>().FirstAsync(expression, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await Context.GetDbSet<T>().ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Context.GetDbSet<T>().Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var ent = await Context.GetDbSet<T>().FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if (ent == null) return;

            Context.GetDbSet<T>().Entry(ent).CurrentValues.SetValues(entity);
        }
    }
}
