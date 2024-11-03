using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories
{
    public sealed class VenueRepository : BaseRepository, IRepository<Venue>
    {
        public VenueRepository(SystemContext context) : base(context)
        {

        }

        public async Task CreateAsync(Venue entity, CancellationToken cancellationToken)
        {
            await Context.Venues.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<Venue, bool>> expression, CancellationToken cancellationToken)
        {
            var enteties = await Context.Venues.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (enteties.Count == 0) return;

            Context.Venues.RemoveRange(enteties);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Venue>> GetListAsync(CancellationToken cancellationToken)
        {
            return await Context.Venues.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Venue>> GetListAsync(Expression<Func<Venue, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Venues.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Venue> GetAsync(Expression<Func<Venue, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Venues.FirstAsync(expression, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Venue entity, CancellationToken cancellationToken)
        {
            var ent = await Context.Venues.FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if (ent == null) return;

            Context.Venues.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
