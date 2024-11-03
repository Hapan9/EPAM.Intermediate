using EPAM.EF.Repositories.Abstraction;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EPAM.EF.Repositories
{
    public sealed class OrderRepository : BaseRepository, IRepository<Order>
    {
        public OrderRepository(SystemContext context) : base(context)
        {
        }

        public async Task CreateAsync(Order entity, CancellationToken cancellationToken)
        {
            await Context.Orders.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<Order, bool>> expression, CancellationToken cancellationToken)
        {
            var enteties = await Context.Orders.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (enteties.Count == 0) return;

            Context.Orders.RemoveRange(enteties);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Order>> GetListAsync(CancellationToken cancellationToken)
        {
            return await Context.Orders.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Order>> GetListAsync(Expression<Func<Order, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Orders.Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Order> GetAsync(Expression<Func<Order, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Orders.FirstAsync(expression, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Order entity, CancellationToken cancellationToken)
        {
            var ent = await Context.Orders.FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
            if (ent == null) return;

            Context.Orders.Entry(ent).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
