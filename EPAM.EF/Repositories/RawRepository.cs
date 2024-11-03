using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories
{
    public sealed class RawRepository : BaseRepository, IRepository<Raw>
    {
        public RawRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(Raw entity, CancellationToken cancellationToken)
        {
            await Context.Raws.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<Raw, bool>> expression, CancellationToken cancellationToken)
        {
            var enteties = await Context.Raws.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (enteties.Count == 0) return;

            Context.Raws.RemoveRange(enteties);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Raw>> GetListAsync(CancellationToken cancellationToken)
        {
            return await Context.Raws.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Raw>> GetListAsync(Expression<Func<Raw, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Raws.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Raw> GetAsync(Expression<Func<Raw, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Raws.FirstAsync(expression, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Raw entity, CancellationToken cancellationToken)
        {
            var ent = await Context.Raws.FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if (ent == null) return;

            Context.Raws.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
