using EPAM.Services.Dtos.Order;

namespace EPAM.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<GetOrderDto>> GetOrdersByCartIdAsync(Guid cartId, CancellationToken cancellationToken);
        Task<List<GetOrderDto>> CreateOrderAsync(Guid cartId, CreateOrderDto createOrderDto, CancellationToken cancellationToken);
        Task DeleteOrderAsync(Guid cartId, Guid eventId, Guid seatId, CancellationToken cancellationToken);
        Task<Guid> BookAllSeatsAsyc(Guid cartId, CancellationToken cancellationToken);
    }
}
