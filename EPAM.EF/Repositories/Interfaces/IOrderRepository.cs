using EPAM.EF.Entities;
using EPAM.EF.Models;

namespace EPAM.EF.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<SeatBooked?> GetBookedSeatAsync(Guid priceOptionId, CancellationToken cancellationToken = default);
        Task<List<SeatBooked>> GetBookedSeatsAsync(Guid cartId, CancellationToken cancellationToken = default);
    }
}
