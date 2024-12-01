using EPAM.Services.Interfaces;
using EPAM.Web.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers
{
    public sealed class VenuesController : BaseController<VenuesController>
    {
        private readonly ISectionService _sectionService;
        private readonly IVenueService _venueService;

        public VenuesController(ISectionService sectionService, IVenueService venueService, ILogger<VenuesController> logger) : base(logger)
        {
            _sectionService = sectionService;
            _venueService = venueService;
        }


        [HttpGet("{id:guid}/sections")]
        public async Task<IActionResult> GetSectionsAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _sectionService.GetSectionsByVenueId(id, cancellationToken).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllVenuesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _venueService.GetListAsync(cancellationToken).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
