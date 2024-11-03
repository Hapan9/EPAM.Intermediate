using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using System.Data;

namespace EPAM.Persistence.UnitOfWork
{
    public sealed class UnitOfWork// : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction? _transaction;
        private IRepository<Event>? _eventRepository;
        private IRepository<Raw>? _rawRepository;
        private IRepository<Seat>? _seatRepository;
        private IRepository<Section>? _sectionRepository;
        private IRepository<Venue>? _venueRepository;

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
        }

        public IRepository<Event> EventRepository => throw new NotImplementedException();// _eventRepository ??= new EventRepository(_connection, _transaction);
        public IRepository<Raw> RawRepository => throw new NotImplementedException();// _rawRepository ??= new RawRepository(_connection, _transaction);
        public IRepository<Seat> SeatRepository => throw new NotImplementedException();// _seatRepository ??= new SeatRepository(_connection, _transaction);
        public IRepository<Section> SectionRepository => throw new NotImplementedException();// _sectionRepository ??= new SectionRepository(_connection, _transaction);
        public IRepository<Venue> VenueRepository => throw new NotImplementedException();// _venueRepository ??= new VenueRepository(_connection, _transaction);

        public IRepository<PriceOption> PriceOptionRepository => throw new NotImplementedException();

        public IRepository<SeatStatus> SeatStatusRepository => throw new NotImplementedException();

        public IRepository<Order> OrderRepository => throw new NotImplementedException();

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            _transaction = _connection.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;
            _connection.Close();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }
    }
}
