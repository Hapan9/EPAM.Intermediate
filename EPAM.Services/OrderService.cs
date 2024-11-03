using AutoMapper;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Entities.Enums;
using EPAM.Persistence.UnitOfWork.Interface;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos.Order;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class OrderService : BaseService<OrderService>, IOrderService
    {
        public OrderService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, ILogger<OrderService> logger) : base(unitOfWorkFactory, mapper, logger)
        {
        }

        public async Task<List<GetOrderDto>> GetOrdersByCartIdAsync(Guid cartId, CancellationToken cancellationToken)
        {
            using var unitOfWork = UnitOfWorkFactory.Create();
            var result = await unitOfWork.OrderRepository.GetListAsync(o => o.CartId == cartId, cancellationToken).ConfigureAwait(false);
            return Mapper.Map<List<GetOrderDto>>(result);
        }

        public async Task<List<GetOrderDto>> CreateOrderAsync(Guid cartId, CreateOrderDto createOrderDto, CancellationToken cancellationToken)
        {
            var order = Mapper.Map<Order>(createOrderDto, opt =>
            {
                opt.AfterMap((_, dest) => dest.CartId = cartId);
            });

            using var unitOfWork = UnitOfWorkFactory.Create();
            await unitOfWork.OrderRepository.CreateAsync(order, cancellationToken).ConfigureAwait(false);
            //var seatStatus = await unitOfWork.SeatStatusRepository.GetAsync(s => s.SeatId == createOrderDto.SeatId, cancellationToken).ConfigureAwait(false);
            //seatStatus.Status = Persistence.Entities.Enums.SeatStatus.Booked;
            //seatStatus.LastStatusChangeDt = DateTime.UtcNow;
            //await unitOfWork.SeatStatusRepository.UpdateAsync(seatStatus, cancellationToken).ConfigureAwait(false);
            var result = await unitOfWork.OrderRepository.GetListAsync(o => o.CartId == cartId, cancellationToken).ConfigureAwait(false);
            return Mapper.Map<List<GetOrderDto>>(result);
        }

        public async Task DeleteOrderAsync(Guid cartId, Guid eventId, Guid seatId, CancellationToken cancellationToken)
        {
            using var unitOfWork = UnitOfWorkFactory.Create();
            await unitOfWork.OrderRepository.DeleteAsync(o => o.CartId == cartId && o.EventId == eventId && o.SeatId == seatId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Guid> BookAllSeatsAsyc(Guid cartId, CancellationToken cancellationToken)
        {
            using var unitOfWork = UnitOfWorkFactory.Create();
            var orders = await unitOfWork.OrderRepository.GetListAsync(o => o.CartId == cartId, cancellationToken).ConfigureAwait(false);
            var seatStatuses = await unitOfWork.SeatStatusRepository.GetListAsync(s => s.Seat!.Orders!.Select(o => o.CartId).Contains(cartId), cancellationToken).ConfigureAwait(false);
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                Status = PaymentStatus.Pending
            };

            unitOfWork.BeginTransaction();

            await unitOfWork.PaymentRepository.CreateAsync(payment, cancellationToken).ConfigureAwait(false);
            foreach (var order in orders)
            {
                order.PaymentId = payment.Id;
                await unitOfWork.OrderRepository.UpdateAsync(order, cancellationToken).ConfigureAwait(false);
            }

            foreach (var seatStatus in seatStatuses)
            {
                seatStatus.Status = Persistence.Entities.Enums.SeatStatus.Booked;
                seatStatus.LastStatusChangeDt = DateTime.UtcNow;
                await unitOfWork.SeatStatusRepository.UpdateAsync(seatStatus, cancellationToken).ConfigureAwait(false);
            }

            unitOfWork.CommitTransaction();

            return payment.Id;
        }
    }
}
