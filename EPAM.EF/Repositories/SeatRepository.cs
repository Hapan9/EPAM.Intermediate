using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories
{
    public sealed class SeatRepository : BaseRepository, IRepository<Seat>
    {
        public SeatRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(Seat entity, CancellationToken cancellationToken)
        {
            await Context.Seats.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<Seat, bool>> expression, CancellationToken cancellationToken)
        {
            var enteties = await Context.Seats.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (enteties.Count == 0) return;

            Context.Seats.RemoveRange(enteties);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Seat>> GetListAsync(CancellationToken cancellationToken)
        {
            return await Context.Seats.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Seat>> GetListAsync(Expression<Func<Seat, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Seats.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Seat> GetAsync(Expression<Func<Seat, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Seats.FirstAsync(expression, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Seat entity, CancellationToken cancellationToken)
        {
            var ent = await Context.Seats.FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if (ent == null) return;

            Context.Seats.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
