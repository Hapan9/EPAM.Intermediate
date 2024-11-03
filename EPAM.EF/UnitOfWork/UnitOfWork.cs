using EPAM.EF.Repositories;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using EPAM.Persistence.UnitOfWork.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace EPAM.EF.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly SystemContext _context;
        private readonly IDbConnection _connection;
        private IDbTransaction? _transaction;
        private IRepository<Event>? _eventRepository;
        private IRepository<Raw>? _rawRepository;
        private IRepository<Seat>? _seatRepository;
        private IRepository<Section>? _sectionRepository;
        private IRepository<Venue>? _venueRepository;
        private IRepository<SeatStatus>? _seatStatusRepository;
        private IRepository<PriceOption>? _priceOptionRepository;
        private IRepository<Order>? _orderRepository;
        private IRepository<Payment>? _paymentRepository;

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
            var options = new DbContextOptionsBuilder<SystemContext>()
                .UseSqlServer((DbConnection)connection)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .Options;
            _context = new SystemContext(options);
        }

        public IRepository<Event> EventRepository => _eventRepository ??= new EventRepository(_context);
        public IRepository<Raw> RawRepository => _rawRepository ??= new RawRepository(_context);
        public IRepository<Seat> SeatRepository => _seatRepository ??= new SeatRepository(_context);
        public IRepository<Section> SectionRepository => _sectionRepository ??= new SectionRepository(_context);
        public IRepository<Venue> VenueRepository => _venueRepository ??= new VenueRepository(_context);
        public IRepository<PriceOption> PriceOptionRepository => _priceOptionRepository ??= new PriceOptionRepository(_context);
        public IRepository<SeatStatus> SeatStatusRepository => _seatStatusRepository ??= new SeatStatusRepository(_context);
        public IRepository<Order> OrderRepository => _orderRepository ??= new OrderRepository(_context);
        public IRepository<Payment> PaymentRepository => _paymentRepository ??= new PaymentRepository(_context);

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            _transaction = _connection.BeginTransaction(isolationLevel);
            _context.Database.UseTransaction((DbTransaction?)_transaction);
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
            _context.Database.UseTransaction((DbTransaction?)_transaction);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;
            _context.Database.UseTransaction((DbTransaction?)_transaction);
            _connection.Close();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
            _context.Database.UseTransaction((DbTransaction?)_transaction);
        }
    }
}
