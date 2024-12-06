using EPAM.Services.Interfaces;
using EPAM.Web.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers
{
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
    public sealed class EventsController : BaseController<EventsController>
    {
        private readonly ISeatService _seatService;
        private readonly IEventService _eventService;

        public EventsController(ISeatService seatService, IEventService eventService, ILogger<EventsController> logger) : base(logger)
        {
            _seatService = seatService;
            _eventService = eventService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _eventService.GetListAsync(cancellationToken).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{eventId:guid}/sections/{sectionId:guid}/seats")]
        public async Task<IActionResult> GetSeatsAsync([FromRoute] Guid eventId, [FromRoute] Guid sectionId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _seatService.GetByEventIdAndSectionIdAsync(eventId, sectionId, cancellationToken).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
