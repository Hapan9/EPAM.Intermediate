﻿using AutoMapper;
using EPAM.Cache.Enums;
using EPAM.Cache.Interfaces;
using EPAM.EF.Entities;
using EPAM.EF.Entities.Enums;
using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos.Order;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class OrderService : BaseService<OrderService>, IOrderService
    {
        const string BookedKey = "SeatBooked";
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
            var cacheResult = await _systemCache.Cache(CacheTypes.DistributedCache).GetAsync<EF.Entities.Enums.SeatStatus?>($"{BookedKey}-{createOrderDto.EventId}-{createOrderDto.SeatId}", cancellationToken);
            if (cacheResult == null || cacheResult == EF.Entities.Enums.SeatStatus.Available)
            {
                var order = Mapper.Map<Order>(createOrderDto, opt =>
                {
                    opt.AfterMap((_, dest) => dest.CartId = cartId);
                });

                await UnitOfWork.OrderRepository.CreateAsync(order, cancellationToken).ConfigureAwait(false);
                await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            Logger.LogInformation($"Seat {createOrderDto.SeatId} is unavailable");

            //var seatStatus = await unitOfWork.SeatStatusRepository.GetAsync(s => s.SeatId == createOrderDto.SeatId, cancellationToken).ConfigureAwait(false);
            //seatStatus.Status = Persistence.Entities.Enums.SeatStatus.Booked;
            //seatStatus.LastStatusChangeDt = DateTime.UtcNow;
            //await unitOfWork.SeatStatusRepository.UpdateAsync(seatStatus, cancellationToken).ConfigureAwait(false);
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

            await UnitOfWork.BeginTransaction(cancellationToken).ConfigureAwait(false);

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
            }
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await UnitOfWork.CommitTransaction(cancellationToken).ConfigureAwait(false);


            foreach (var seatStatus in seatStatuses)
            {
                await _systemCache.Cache(CacheTypes.DistributedCache).Set($"{BookedKey}-{seatStatus.EventId}-{seatStatus.SeatId}", EF.Entities.Enums.SeatStatus.Booked, cancellationToken);
            }

            return payment.Id;
        }
    }
}
