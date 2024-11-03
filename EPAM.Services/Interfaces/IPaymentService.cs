using EPAM.Services.Dtos;

namespace EPAM.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<PaymentDto> GetPaymentAsync(Guid id, CancellationToken cancellationToken);

        public Task UpdateStatusToCompleteAsync(Guid id, CancellationToken cancellationToken);
        public Task UpdateStatusToFailedAsync(Guid id, CancellationToken cancellationToken);
    }
}
