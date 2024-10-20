﻿using EPAM.EF.Entities;
using EPAM.EF.Repositories;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Interfaces;
using EPAM.Persistence.UnitOfWork.Interface;
using Microsoft.EntityFrameworkCore;
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

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
            var options = new DbContextOptionsBuilder<SystemContext>()
                .UseSqlServer((DbConnection)connection)
                .Options;
            _context = new SystemContext(options);
        }

        public IRepository<Event> EventRepository => _eventRepository ??= new EventRepository(_context);
        public IRepository<Raw> RawRepository => _rawRepository ??= new RawRepository(_context);
        public IRepository<Seat> SeatRepository => _seatRepository ??= new SeatRepository(_context);
        public IRepository<Section> SectionRepository => _sectionRepository ??= new SectionRepository(_context);
        public IRepository<Venue> VenueRepository => _venueRepository ??= new VenueRepository(_context);

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
            Dispose();
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
            Dispose();
        }
    }
}
