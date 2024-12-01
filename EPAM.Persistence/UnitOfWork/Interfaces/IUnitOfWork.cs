using EPAM.EF.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using System.Data;

namespace EPAM.Persistence.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void BeginTransaction(IsolationLevel isolationLevel);
        void CommitTransaction();
        void RollbackTransaction();

        IRepository<Event> EventRepository { get; }
        IRepository<Raw> RawRepository { get; }
        IRepository<Seat> SeatRepository { get; }
        IRepository<Section> SectionRepository { get; }
        IRepository<Venue> VenueRepository { get; }
    }
}
