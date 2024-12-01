using AutoMapper;
using EPAM.EF.Entities.Enums;
using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class PaymentService : BaseService<PaymentService>, IPaymentService
    {
        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PaymentService> logger) : base(unitOfWork, mapper, logger)
        {
        }

        public async Task<PaymentDto> GetPaymentAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await UnitOfWork.PaymentRepository.GetAsync(p => p.Id == id, cancellationToken).ConfigureAwait(false);
            var priceOptions = await UnitOfWork.PriceOptionRepository.GetListAsync(p => p.Order!.PaymentId == id, cancellationToken).ConfigureAwait(false);
            return Mapper.Map<PaymentDto>(result, opt =>
            {
                opt.AfterMap((_, dest) => dest.Price = priceOptions.Select(p => p.Price).Sum());
            });
        }

        public async Task UpdateStatusToCompleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var payment = await UnitOfWork.PaymentRepository.GetAsync(p => p.Id == id, cancellationToken).ConfigureAwait(false);
            var seatsStatuses = await UnitOfWork.SeatStatusRepository.GetListAsync(s => s.Seat!.Orders!.Select(o => o.PaymentId).Contains(id), cancellationToken).ConfigureAwait(false);

            await UnitOfWork.BeginTransaction(cancellationToken).ConfigureAwait(false);

            payment.Status = PaymentStatus.Completed;
            await UnitOfWork.PaymentRepository.UpdateAsync(payment, cancellationToken).ConfigureAwait(false);
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            foreach (var seatStatus in seatsStatuses)
            {
                seatStatus.Status = SeatStatus.Sold;
                seatStatus.LastStatusChangeDt = DateTime.UtcNow;
                await UnitOfWork.SeatStatusRepository.UpdateAsync(seatStatus, cancellationToken).ConfigureAwait(false);
            }
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await UnitOfWork.CommitTransaction(cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateStatusToFailedAsync(Guid id, CancellationToken cancellationToken)
        {
            var payment = await UnitOfWork.PaymentRepository.GetAsync(p => p.Id == id, cancellationToken).ConfigureAwait(false);
            var seatsStatuses = await UnitOfWork.SeatStatusRepository.GetListAsync(s => s.Seat!.Orders!.Select(o => o.PaymentId).Contains(id), cancellationToken).ConfigureAwait(false);

            await UnitOfWork.BeginTransaction(cancellationToken).ConfigureAwait(false);

            payment.Status = PaymentStatus.Declined;
            await UnitOfWork.PaymentRepository.UpdateAsync(payment, cancellationToken).ConfigureAwait(false);
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            foreach (var seatStatus in seatsStatuses)
            {
                seatStatus.Status = SeatStatus.Available;
                seatStatus.LastStatusChangeDt = DateTime.UtcNow;
                await UnitOfWork.SeatStatusRepository.UpdateAsync(seatStatus, cancellationToken).ConfigureAwait(false);
            }
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await UnitOfWork.CommitTransaction(cancellationToken).ConfigureAwait(false);
        }
    }
}
