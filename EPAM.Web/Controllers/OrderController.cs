using EPAM.Services.Dtos.Order;
using EPAM.Services.Interfaces;
using EPAM.Web.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers
{
    public sealed class OrderController : BaseController<OrderController>
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger) : base(logger)
        {
            _orderService = orderService;
        }

        [HttpGet("cart/{cartId:guid}")]
        public async Task<IActionResult> GetOrdersByCartIdAsync([FromRoute] Guid cartId, CancellationToken cancellationToken)
        {
            var result = await _orderService.GetOrdersByCartIdAsync(cartId, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("cart/{cartId:guid}")]
        public async Task<IActionResult> CreateOrdersForCartAsync([FromRoute] Guid cartId, [FromBody] CreateOrderDto createOrderDto, CancellationToken cancellationToken)
        {
            var result = await _orderService.CreateOrderAsync(cartId, createOrderDto, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpDelete("cart/{cartId:guid}/events/{eventId:guid}/seats/{seatId:guid}")]
        public async Task<IActionResult> DeleteOrdersForCartAsync([FromRoute] Guid cartId, [FromRoute] Guid eventId, [FromRoute] Guid seatId, CancellationToken cancellationToken)
        {
            await _orderService.DeleteOrderAsync(cartId, eventId, seatId, cancellationToken).ConfigureAwait(false);
            return Ok();
        }

        [HttpPut("cart/{cartId:guid}/book")]
        public async Task<IActionResult> BookSeatsForCartAsync([FromRoute] Guid cartId, CancellationToken cancellationToken)
        {
            var result = await _orderService.BookAllSeatsAsyc(cartId, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
