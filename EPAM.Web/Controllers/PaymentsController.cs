using EPAM.Services.Interfaces;
using EPAM.Web.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers
{
    public sealed class PaymentsController : BaseController<PaymentsController>
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger) : base(logger)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPaymentAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _paymentService.GetPaymentAsync(id, cancellationToken).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //[HttpPost("{id:guid}/completed")]
        [HttpPut("{id:guid}/completed")]
        public async Task<IActionResult> UpdateStatusToCompleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _paymentService.UpdateStatusToCompleteAsync(id, cancellationToken).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //[HttpPost("{id:guid}/failed")]
        [HttpPut("{id:guid}/failed")]
        public async Task<IActionResult> UpdateStatusToFailedAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _paymentService.UpdateStatusToFailedAsync(id, cancellationToken).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
