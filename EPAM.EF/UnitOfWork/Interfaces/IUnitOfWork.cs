using EPAM.EF.Entities;
using EPAM.EF.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace EPAM.EF.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransaction(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
        Task CommitTransaction(CancellationToken cancellationToken = default);
        Task RollbackTransaction(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IRepository<Event> EventRepository { get; }
        IRepository<Raw> RawRepository { get; }
        IRepository<Seat> SeatRepository { get; }
        IRepository<Section> SectionRepository { get; }
        IRepository<Venue> VenueRepository { get; }
        IRepository<PriceOption> PriceOptionRepository { get; }
        IRepository<SeatStatus> SeatStatusRepository { get; }
        IRepository<Order> OrderRepository { get; }
        IRepository<Payment> PaymentRepository { get; }
    }
}
