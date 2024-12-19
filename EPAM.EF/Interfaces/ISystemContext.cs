using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace EPAM.EF.Interfaces
{
    public interface ISystemContext
    {
        DbSet<Event> Events { get; set; }

        DbSet<Raw> Raws { get; set; }

        DbSet<Seat> Seats { get; set; }

        DbSet<Section> Sections { get; set; }

        DbSet<Venue> Venues { get; set; }

        DbSet<PriceOption> PriceOptions { get; set; }

        DbSet<SeatStatus> SeatsStatuses { get; set; }

        DbSet<Order> Orders { get; set; }

        DbSet<Payment> Payments { get; set; }

        DbSet<Notification> Notifications { get; set; }

        DbSet<NotificationParam> NotificationsParams { get; set; }

        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);

        DbSet<T> GetDbSet<T>() where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
