using EPAM.Services.Interfaces;
using EPAM.Web.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers
{
    public sealed class EventController : BaseController<EventController>
    {
        private readonly ISeatService _seatService;
        private readonly IEventService _eventService;

        public EventController(ISeatService seatService, IEventService eventService, ILogger<EventController> logger) : base(logger)
        {
            _seatService = seatService;
            _eventService = eventService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            var result = await _eventService.GetListAsync(cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("{eventId:guid}/sections/{sectionId:guid}/seats")]
        public async Task<IActionResult> GetAllEventsAsync([FromRoute] Guid eventId, [FromRoute] Guid sectionId, CancellationToken cancellationToken)
        {
            var result = await _seatService.GetByEventIdAndSectionIdAsync(eventId, sectionId, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
