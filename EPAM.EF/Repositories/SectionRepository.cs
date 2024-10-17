using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.Repositories
{
    public sealed class SectionRepository : BaseRepository, IRepository<Section>
    {
        public SectionRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(Section entity)
        {
            await Context.Sections.AddAsync(entity).ConfigureAwait(false);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await Context.Sections.FindAsync(id).ConfigureAwait(false);
            if (entity == null) return;

            Context.Sections.Remove(entity);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Section>> GetAllAsync()
        {
            return await Context.Sections.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Section> GetAsync(Guid id)
        {
            return await Context.Sections.FirstAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Section entity)
        {
            var ent = await Context.Sections.FindAsync(entity.Id).ConfigureAwait(false);
            if (ent == null) return;

            Context.Sections.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
