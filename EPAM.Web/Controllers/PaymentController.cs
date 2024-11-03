using EPAM.Services.Interfaces;
using EPAM.Web.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers
{
    public sealed class PaymentController : BaseController<PaymentController>
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger) : base(logger)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPaymentAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _paymentService.GetPaymentAsync(id, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        //[HttpPost("{id:guid}/completed")]
        [HttpPut("{id:guid}/completed")]
        public async Task<IActionResult> UpdateStatusToCompleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _paymentService.UpdateStatusToCompleteAsync(id, cancellationToken).ConfigureAwait(false);
            return Ok();
        }

        //[HttpPost("{id:guid}/failed")]
        [HttpPut("{id:guid}/failed")]
        public async Task<IActionResult> UpdateStatusToFailedAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _paymentService.UpdateStatusToFailedAsync(id, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
    }
}
