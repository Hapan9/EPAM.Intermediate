using EPAM.EF.Entities;
using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.Repositories
{
    public sealed class SeatRepository : BaseRepository, IRepository<Seat>
    {
        public SeatRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(Seat entity)
        {
            await Context.Seats.AddAsync(entity).ConfigureAwait(false);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await Context.Seats.FindAsync(id).ConfigureAwait(false);
            if (entity == null) return;

            Context.Remove(entity);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Seat>> GetAllAsync()
        {
            return await Context.Seats.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Seat> GetAsync(Guid id)
        {
            return await Context.Seats.FirstAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Seat entity)
        {
            var ent = await Context.Seats.FindAsync(entity.Id).ConfigureAwait(false);
            if (ent == null) return;

            Context.Seats.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
