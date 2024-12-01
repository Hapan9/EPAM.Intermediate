using EPAM.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        DbSet<T> GetDbSet<T>() where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
