using EPAM.EF.Entities;
using EPAM.EF.Repositories.Interfaces;

namespace EPAM.EF.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransaction(CancellationToken cancellationToken);
        Task CommitTransaction(CancellationToken cancellationToken);
        Task RollbackTransaction(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

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
