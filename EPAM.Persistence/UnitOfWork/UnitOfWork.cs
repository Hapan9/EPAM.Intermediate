using EPAM.EF.Entities;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories;
using EPAM.Persistence.Repositories.Interfaces;
using EPAM.Persistence.UnitOfWork.Interface;
using System.Data;

namespace EPAM.Persistence.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
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

        public IRepository<Event> EventRepository => _eventRepository ??= new EventRepository(_connection, _transaction);
        public IRepository<Raw> RawRepository => _rawRepository ??= new RawRepository(_connection, _transaction);
        public IRepository<Seat> SeatRepository => _seatRepository ??= new SeatRepository(_connection, _transaction);
        public IRepository<Section> SectionRepository => _sectionRepository ??= new SectionRepository(_connection, _transaction);
        public IRepository<Venue> VenueRepository => _venueRepository ??= new VenueRepository(_connection, _transaction);

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
            Dispose();
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
            Dispose();
        }
    }
}
