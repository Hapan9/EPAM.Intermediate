using EPAM.Services.Dtos.Order;
using EPAM.Services.Interfaces;
using EPAM.Web.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers
{
    public sealed class OrdersController : BaseController<OrdersController>
    {
        private readonly IOrderService _orderService;
        private readonly INotificationService _notificationService;

        public OrdersController(IOrderService orderService, INotificationService notificationService, ILogger<OrdersController> logger) : base(logger)
        {
            _orderService = orderService;
            _notificationService = notificationService;
        }

        [HttpGet("carts/{cartId:guid}")]
        public async Task<IActionResult> GetOrdersByCartIdAsync([FromRoute] Guid cartId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _orderService.GetOrdersByCartIdAsync(cartId, cancellationToken).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("carts/{cartId:guid}")]
        public async Task<IActionResult> CreateOrdersForCartAsync([FromRoute] Guid cartId, [FromBody] CreateOrderDto createOrderDto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _orderService.CreateOrderAsync(cartId, createOrderDto, cancellationToken).ConfigureAwait(false);
                await _notificationService.NotifySeatBooked(createOrderDto.PriceOptionId, cancellationToken).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("carts/{cartId:guid}/events/{eventId:guid}/seats/{seatId:guid}")]
        public async Task<IActionResult> DeleteOrdersForCartAsync([FromRoute] Guid cartId, [FromRoute] Guid eventId, [FromRoute] Guid seatId, CancellationToken cancellationToken)
        {
            try
            {
                await _orderService.DeleteOrderAsync(cartId, eventId, seatId, cancellationToken).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("carts/{cartId:guid}/book")]
        public async Task<IActionResult> BookSeatsForCartAsync([FromRoute] Guid cartId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _orderService.BookAllSeatsAsyc(cartId, cancellationToken).ConfigureAwait(false);
                await _notificationService.NotifySeatsBooked(cartId, cancellationToken).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
