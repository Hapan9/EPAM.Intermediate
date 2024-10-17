using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.Repositories
{
    public sealed class RawRepository : BaseRepository, IRepository<Raw>
    {
        public RawRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(Raw entity)
        {
            await Context.Raws.AddAsync(entity).ConfigureAwait(false);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await Context.Raws.FindAsync(id).ConfigureAwait(false);
            if (entity == null) return;

            Context.Raws.Remove(entity);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Raw>> GetAllAsync()
        {
            return await Context.Raws.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Raw> GetAsync(Guid id)
        {
            return await Context.Raws.FirstAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Raw entity)
        {
            var ent = await Context.Raws.FindAsync(entity.Id).ConfigureAwait(false);
            if (ent == null) return;

            Context.Raws.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
