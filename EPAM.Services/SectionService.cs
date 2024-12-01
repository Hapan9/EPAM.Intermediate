using AutoMapper;
using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class SectionService : BaseService<SectionService>, ISectionService
    {
        public SectionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SectionService> logger) : base(unitOfWork, mapper, logger)
        {
        }

        public async Task<List<SectionDto>> GetSectionsByVenueId(Guid venueId, CancellationToken cancellationToken)
        {
            var result = await UnitOfWork.SectionRepository.GetListAsync(s => s.VenueId == venueId, cancellationToken).ConfigureAwait(false);
            return Mapper.Map<List<SectionDto>>(result);
        }
    }
}
