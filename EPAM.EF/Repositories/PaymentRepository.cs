using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories
{
    public sealed class PaymentRepository : BaseRepository, IRepository<Payment>
    {
        public PaymentRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(Payment entity, CancellationToken cancellationToken)
        {
            await Context.Payments.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<Payment, bool>> expression, CancellationToken cancellationToken)
        {
            var enteties = await Context.Payments.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (enteties.Count == 0) return;

            Context.Payments.RemoveRange(enteties);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Payment>> GetListAsync(CancellationToken cancellationToken)
        {
            return await Context.Payments.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Payment>> GetListAsync(Expression<Func<Payment, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Payments.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Payment> GetAsync(Expression<Func<Payment, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Payments.FirstAsync(expression, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Payment entity, CancellationToken cancellationToken)
        {
            var ent = await Context.Payments.FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if (ent == null) return;

            Context.Payments.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
