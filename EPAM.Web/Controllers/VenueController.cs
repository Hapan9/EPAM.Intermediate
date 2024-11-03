using EPAM.Services.Interfaces;
using EPAM.Web.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers
{
    public sealed class VenueController : BaseController<VenueController>
    {
        private readonly ISectionService _sectionService;
        private readonly IVenueService _venueService;

        public VenueController(ISectionService sectionService, IVenueService venueService, ILogger<VenueController> logger) : base(logger)
        {
            _sectionService = sectionService;
            _venueService = venueService;
        }


        [HttpGet("{id:guid}/sections")]
        public async Task<IActionResult> GetSectionsAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _sectionService.GetSectionsByVenueId(id, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllVenuesAsync(CancellationToken cancellationToken)
        {
            var result = await _venueService.GetListAsync(cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
