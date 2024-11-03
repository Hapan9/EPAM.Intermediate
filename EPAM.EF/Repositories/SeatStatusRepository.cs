using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories
{
    public class SeatStatusRepository : BaseRepository, IRepository<SeatStatus>
    {
        public SeatStatusRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(SeatStatus entity, CancellationToken cancellationToken)
        {
            await Context.SeatsStatuses.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<SeatStatus, bool>> expression, CancellationToken cancellationToken)
        {
            var enteties = await Context.SeatsStatuses.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (enteties.Count == 0) return;

            Context.SeatsStatuses.RemoveRange(enteties);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<SeatStatus>> GetListAsync(Expression<Func<SeatStatus, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.SeatsStatuses.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<SeatStatus>> GetListAsync(CancellationToken cancellationToken)
        {
            return await Context.SeatsStatuses.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<SeatStatus> GetAsync(Expression<Func<SeatStatus, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.SeatsStatuses.FirstAsync(expression, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(SeatStatus entity, CancellationToken cancellationToken)
        {
            var ent = await Context.SeatsStatuses.FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if (ent == null) return;

            Context.SeatsStatuses.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
