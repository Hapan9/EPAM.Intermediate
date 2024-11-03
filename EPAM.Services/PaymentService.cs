using AutoMapper;
using EPAM.Persistence.UnitOfWork.Interface;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class PaymentService : BaseService<PaymentService>, IPaymentService
    {
        public PaymentService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, ILogger<PaymentService> logger) : base(unitOfWorkFactory, mapper, logger)
        {
        }

        public async Task<PaymentDto> GetPaymentAsync(Guid id, CancellationToken cancellationToken)
        {
            using var unitOfWork = UnitOfWorkFactory.Create();
            var result = await unitOfWork.PaymentRepository.GetAsync(p => p.Id == id, cancellationToken).ConfigureAwait(false);
            var priceOptions = await unitOfWork.PriceOptionRepository.GetListAsync(p => p.Order!.PaymentId == id, cancellationToken).ConfigureAwait(false);
            return Mapper.Map<PaymentDto>(result, opt =>
            {
                opt.AfterMap((_, dest) => dest.Price = priceOptions.Select(p => p.Price).Sum());
            });
        }

        public async Task UpdateStatusToCompleteAsync(Guid id, CancellationToken cancellationToken)
        {
            using var unitOfWork = UnitOfWorkFactory.Create();
            var payment = await unitOfWork.PaymentRepository.GetAsync(p => p.Id == id, cancellationToken).ConfigureAwait(false);
            var seatsStatuses = await unitOfWork.SeatStatusRepository.GetListAsync(s => s.Seat!.Orders!.Select(o => o.PaymentId).Contains(id), cancellationToken).ConfigureAwait(false);
            unitOfWork.BeginTransaction();

            payment.Status = Persistence.Entities.Enums.PaymentStatus.Completed;
            await unitOfWork.PaymentRepository.UpdateAsync(payment, cancellationToken).ConfigureAwait(false);

            foreach (var seatStatus in seatsStatuses)
            {
                seatStatus.Status = Persistence.Entities.Enums.SeatStatus.Sold;
                seatStatus.LastStatusChangeDt = DateTime.UtcNow;
                await unitOfWork.SeatStatusRepository.UpdateAsync(seatStatus, cancellationToken).ConfigureAwait(false);
            }
            unitOfWork.CommitTransaction();
        }

        public async Task UpdateStatusToFailedAsync(Guid id, CancellationToken cancellationToken)
        {
            using var unitOfWork = UnitOfWorkFactory.Create();
            var payment = await unitOfWork.PaymentRepository.GetAsync(p => p.Id == id, cancellationToken).ConfigureAwait(false);
            var seatsStatuses = await unitOfWork.SeatStatusRepository.GetListAsync(s => s.Seat!.Orders!.Select(o => o.PaymentId).Contains(id), cancellationToken).ConfigureAwait(false);
            unitOfWork.BeginTransaction();

            payment.Status = Persistence.Entities.Enums.PaymentStatus.Declined;
            await unitOfWork.PaymentRepository.UpdateAsync(payment, cancellationToken).ConfigureAwait(false);

            foreach (var seatStatus in seatsStatuses)
            {
                seatStatus.Status = Persistence.Entities.Enums.SeatStatus.Available;
                seatStatus.LastStatusChangeDt = DateTime.UtcNow;
                await unitOfWork.SeatStatusRepository.UpdateAsync(seatStatus, cancellationToken).ConfigureAwait(false);
            }
            unitOfWork.CommitTransaction();
        }
    }
}
