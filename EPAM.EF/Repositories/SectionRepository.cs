using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories
{
    public sealed class SectionRepository : BaseRepository, IRepository<Section>
    {
        public SectionRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(Section entity, CancellationToken cancellationToken)
        {
            await Context.Sections.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<Section, bool>> expression, CancellationToken cancellationToken)
        {
            var enteties = await Context.Sections.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (enteties.Count == 0) return;

            Context.Sections.RemoveRange(enteties);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Section>> GetListAsync(CancellationToken cancellationToken)
        {
            return await Context.Sections.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Section>> GetListAsync(Expression<Func<Section, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Sections.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Section> GetAsync(Expression<Func<Section, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Sections.FirstAsync(expression, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Section entity, CancellationToken cancellationToken)
        {
            var ent = await Context.Sections.FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if (ent == null) return;

            Context.Sections.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
