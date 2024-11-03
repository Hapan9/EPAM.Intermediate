using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories
{
    public sealed class PriceOptionRepository : BaseRepository, IRepository<PriceOption>
    {
        public PriceOptionRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(PriceOption entity, CancellationToken cancellationToken)
        {
            await Context.PriceOptions.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<PriceOption, bool>> expression, CancellationToken cancellationToken)
        {
            var enteties = await Context.PriceOptions.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (enteties.Count == 0) return;

            Context.PriceOptions.RemoveRange(enteties);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<PriceOption>> GetListAsync(CancellationToken cancellationToken)
        {
            return await Context.PriceOptions.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<PriceOption>> GetListAsync(Expression<Func<PriceOption, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.PriceOptions.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<PriceOption> GetAsync(Expression<Func<PriceOption, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.PriceOptions.FirstAsync(expression, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(PriceOption entity, CancellationToken cancellationToken)
        {
            var ent = await Context.PriceOptions.FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if (ent == null) return;

            Context.PriceOptions.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
