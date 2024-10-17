using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.Repositories
{
    public sealed class EventRepository : BaseRepository, IRepository<Event>
    {
        public EventRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(Event entity)
        {
            await Context.Events.AddAsync(entity).ConfigureAwait(false);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await Context.Events.FindAsync(id).ConfigureAwait(false);
            if (entity == null) return;

            Context.Events.Remove(entity);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await Context.Events.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Event> GetAsync(Guid id)
        {
            return await Context.Events.FirstAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Event entity)
        {
            var ent = await Context.Venues.FindAsync(entity.Id).ConfigureAwait(false);
            if (ent == null) return;

            Context.Venues.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
