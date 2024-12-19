using EPAM.EF.Entities;
using EPAM.EF.Interfaces;
using EPAM.EF.Repositories;
using EPAM.EF.Repositories.Abstraction;
using EPAM.EF.Repositories.Interfaces;
using EPAM.EF.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace EPAM.EF.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ISystemContext _context;
        private IDbContextTransaction? _transaction;
        private IRepository<Event>? _eventRepository;
        private IRepository<Raw>? _rawRepository;
        private IRepository<Seat>? _seatRepository;
        private IRepository<Section>? _sectionRepository;
        private IRepository<Venue>? _venueRepository;
        private IRepository<SeatStatus>? _seatStatusRepository;
        private IRepository<PriceOption>? _priceOptionRepository;
        private IOrderRepository? _orderRepository;
        private IRepository<Payment>? _paymentRepository;
        private INotificationRepository? _notificationRepository;

        public UnitOfWork(ISystemContext context)
        {
            _context = context;
        }

        public IRepository<Event> EventRepository => _eventRepository ??= new BaseRepository<Event>(_context);
        public IRepository<Raw> RawRepository => _rawRepository ??= new BaseRepository<Raw>(_context);
        public IRepository<Seat> SeatRepository => _seatRepository ??= new BaseRepository<Seat>(_context);
        public IRepository<Section> SectionRepository => _sectionRepository ??= new BaseRepository<Section>(_context);
        public IRepository<Venue> VenueRepository => _venueRepository ??= new BaseRepository<Venue>(_context);
        public IRepository<PriceOption> PriceOptionRepository => _priceOptionRepository ??= new BaseRepository<PriceOption>(_context);
        public IRepository<SeatStatus> SeatStatusRepository => _seatStatusRepository ??= new BaseRepository<SeatStatus>(_context);
        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_context);
        public IRepository<Payment> PaymentRepository => _paymentRepository ??= new BaseRepository<Payment>(_context);
        public INotificationRepository NotificationRepository => _notificationRepository ??= new NotificationRepository(_context);

        public async Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            _transaction = await _context.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(false);
            return _transaction;
        }

        public async Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            await BeginTransaction(IsolationLevel.ReadCommitted, cancellationToken).ConfigureAwait(false);
        }

        public async Task CommitTransaction(CancellationToken cancellationToken = default)
        {
            if (_transaction == null) return;

            await _transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            await _transaction.DisposeAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }

        public async Task RollbackTransaction(CancellationToken cancellationToken = default)
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            await _transaction.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
