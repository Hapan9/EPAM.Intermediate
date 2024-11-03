using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories
{
    public sealed class EventRepository : BaseRepository, IRepository<Event>
    {
        public EventRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(Event entity, CancellationToken cancellationToken)
        {
            await Context.Events.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken)
        {
            var enteties = await Context.Events.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (enteties.Count == 0) return;

            Context.Events.RemoveRange(enteties);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Event>> GetListAsync(CancellationToken cancellationToken)
        {
            return await Context.Events.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Event>> GetListAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Events.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Event> GetAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Events.FirstAsync(expression, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Event entity, CancellationToken cancellationToken)
        {
            var ent = await Context.Venues.FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if (ent == null) return;

            Context.Venues.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
