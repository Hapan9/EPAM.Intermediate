using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.Repositories
{
    public sealed class VenueRepository : BaseRepository, IRepository<Venue>
    {
        public VenueRepository(SystemContext context) : base(context)
        {

        }

        public async Task CreateAsync(Venue entity)
        {
            await Context.Venues.AddAsync(entity).ConfigureAwait(false);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await Context.Venues.FindAsync(id).ConfigureAwait(false);
            if (entity == null) return;

            Context.Venues.Remove(entity);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Venue>> GetAllAsync()
        {
            return await Context.Venues.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Venue> GetAsync(Guid id)
        {
            return await Context.Venues.FirstAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Venue entity)
        {
            var ent = await Context.Venues.FindAsync(entity.Id).ConfigureAwait(false);
            if (ent == null) return;

            Context.Venues.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
