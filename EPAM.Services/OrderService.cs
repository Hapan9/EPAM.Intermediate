using AutoMapper;
using EPAM.Cache.Enums;
using EPAM.Cache.Interfaces;
using EPAM.EF.Entities;
using EPAM.EF.Entities.Enums;
using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos.Order;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace EPAM.Services
{
    public sealed class OrderService : BaseService<OrderService>, IOrderService
    {
        const string BookedKey = "SeatBooked";
        const CacheTypes CacheType = CacheTypes.MemoryCache;
        private readonly ISystemCache _systemCache;

        public OrderService(ISystemCache systemCache, IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger) : base(unitOfWork, mapper, logger)
        {
            _systemCache = systemCache;
        }

        public async Task<List<GetOrderDto>> GetOrdersByCartIdAsync(Guid cartId, CancellationToken cancellationToken)
        {
            var result = await UnitOfWork.OrderRepository.GetListAsync(o => o.CartId == cartId, cancellationToken).ConfigureAwait(false);
            return Mapper.Map<List<GetOrderDto>>(result);
        }

        public async Task<List<GetOrderDto>> CreateOrderAsync(Guid cartId, CreateOrderDto createOrderDto, CancellationToken cancellationToken)
        {
            var cacheResult = await _systemCache.GetCache(CacheType).GetAsync<EF.Entities.Enums.SeatStatus?>($"{BookedKey}-{createOrderDto.EventId}-{createOrderDto.SeatId}", cancellationToken);

            if (cacheResult != null && cacheResult != EF.Entities.Enums.SeatStatus.Available)
            {
                Logger.LogInformation($"Seat {createOrderDto.SeatId} is unavailable");
                throw new Exception($"Seat {createOrderDto.SeatId} is unavailable");
            }

            await UnitOfWork.BeginTransaction(IsolationLevel.RepeatableRead, cancellationToken);

            var seatStatus = await UnitOfWork.SeatStatusRepository
                    .GetAsync(s => s.SeatId == createOrderDto.SeatId && s.EventId == createOrderDto.EventId)
                    .ConfigureAwait(false);

            if (seatStatus.Status != EF.Entities.Enums.SeatStatus.Available)
            {
                await UnitOfWork.RollbackTransaction(cancellationToken);

                await _systemCache.GetCache(CacheType).SetAsync($"{BookedKey}-{seatStatus.EventId}-{seatStatus.SeatId}", seatStatus.Status, cancellationToken);
                Logger.LogInformation($"Seat {createOrderDto.SeatId} is unavailable");
                throw new Exception($"Seat {createOrderDto.SeatId} is unavailable");
            }

            var order = Mapper.Map<Order>(createOrderDto, opt =>
            {
                opt.AfterMap((_, dest) => dest.CartId = cartId);
            });

            await UnitOfWork.OrderRepository.CreateAsync(order, cancellationToken).ConfigureAwait(false);
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            seatStatus.Status = EF.Entities.Enums.SeatStatus.Booked;
            seatStatus.LastStatusChangeDt = DateTime.UtcNow;
            seatStatus.Version = Guid.NewGuid();

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            await UnitOfWork.CommitTransaction(cancellationToken);

            await _systemCache.GetCache(CacheType).SetAsync($"{BookedKey}-{seatStatus.EventId}-{seatStatus.SeatId}", seatStatus.Status, cancellationToken);

            var result = await UnitOfWork.OrderRepository.GetListAsync(o => o.CartId == cartId, cancellationToken).ConfigureAwait(false);

            return Mapper.Map<List<GetOrderDto>>(result);
        }

        public async Task DeleteOrderAsync(Guid cartId, Guid eventId, Guid seatId, CancellationToken cancellationToken)
        {
            await UnitOfWork.OrderRepository.DeleteAsync(o => o.CartId == cartId && o.EventId == eventId && o.SeatId == seatId, cancellationToken).ConfigureAwait(false);
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Guid> BookAllSeatsAsyc(Guid cartId, CancellationToken cancellationToken)
        {
            var orders = await UnitOfWork.OrderRepository.GetListAsync(o => o.CartId == cartId, cancellationToken).ConfigureAwait(false);
            var seatStatuses = await UnitOfWork.SeatStatusRepository.GetListAsync(s => s.Seat!.Orders!.Select(o => o.CartId).Contains(cartId), cancellationToken).ConfigureAwait(false);
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                Status = PaymentStatus.Pending
            };

            await UnitOfWork.BeginTransaction(IsolationLevel.RepeatableRead, cancellationToken).ConfigureAwait(false);

            await UnitOfWork.PaymentRepository.CreateAsync(payment, cancellationToken).ConfigureAwait(false);
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            foreach (var order in orders)
            {
                order.PaymentId = payment.Id;
            }
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            foreach (var seatStatus in seatStatuses)
            {
                seatStatus.Status = EF.Entities.Enums.SeatStatus.Booked;
                seatStatus.LastStatusChangeDt = DateTime.UtcNow;
                seatStatus.Version = Guid.NewGuid();
            }
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await UnitOfWork.CommitTransaction(cancellationToken).ConfigureAwait(false);


            foreach (var seatStatus in seatStatuses)
            {
                await _systemCache.GetCache(CacheType).SetAsync($"{BookedKey}-{seatStatus.EventId}-{seatStatus.SeatId}", EF.Entities.Enums.SeatStatus.Booked, cancellationToken);
            }

            return payment.Id;
        }
    }
}
