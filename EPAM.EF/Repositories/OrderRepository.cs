using EPAM.EF.Entities;
using EPAM.EF.Interfaces;
using EPAM.EF.Models;
using EPAM.EF.Repositories.Abstraction;
using EPAM.EF.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPAM.EF.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ISystemContext context) : base(context)
        {
        }

        public async Task<SeatBooked?> GetBookedSeatAsync(Guid priceOptionId, CancellationToken cancellationToken = default)
        {
            return await Context.PriceOptions
                .Where(p => p.Id == priceOptionId)
                .Where(p => p.Seat!.SeatStatuses!.Where(s => s.EventId == p.EventId && s.Status == Entities.Enums.SeatStatus.Booked).Any())
                .Select(p => new SeatBooked
                {
                    EventName = p.Event!.Name,
                    SeatNumber = p.Seat!.Number,
                    RawNumber = p.Seat.Raw!.Number,
                    SectionName = p.Seat.Raw.Section!.Name,
                    VenueName = p.Event.Venue!.Name,
                    Price = p.Price
                }
                ).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<SeatBooked>> GetBookedSeatsAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            return await Context.Orders
                .Where(o => o.CartId == cartId)
                .Where(o => o.Seat!.SeatStatuses!.All(s => s.Status == Entities.Enums.SeatStatus.Booked && s.EventId == o.EventId))
                .Select(o => new SeatBooked
                {
                    EventName = o.Event!.Name,
                    VenueName = o.Event!.Venue!.Name,
                    SectionName = o.Seat!.Raw!.Section!.Name,
                    RawNumber = o.Seat.Raw.Number,
                    SeatNumber = o.Seat.Number,
                    Price = o.PriceOption!.Price
                }).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<SeatBooked>> GetSeatAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            return await Context.Orders
                .Where(o => o.CartId == cartId)
                .Where(o => o.Seat!.SeatStatuses!.All(s => s.Status == Entities.Enums.SeatStatus.Booked && s.EventId == o.EventId))
                .Select(o => new SeatBooked
                {
                    EventName = o.Event!.Name,
                    VenueName = o.Event!.Venue!.Name,
                    SectionName = o.Seat!.Raw!.Section!.Name,
                    RawNumber = o.Seat.Raw.Number,
                    SeatNumber = o.Seat.Number,
                    Price = o.PriceOption!.Price
                }).ToListAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
