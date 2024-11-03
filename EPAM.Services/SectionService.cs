using AutoMapper;
using EPAM.Persistence.UnitOfWork.Interface;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class SectionService : BaseService<SectionService>, ISectionService
    {
        public SectionService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, ILogger<SectionService> logger) : base(unitOfWorkFactory, mapper, logger)
        {
        }

        public async Task<List<SectionDto>> GetSectionsByVenueId(Guid venueId, CancellationToken cancellationToken)
        {
            using var unitOfWork = UnitOfWorkFactory.Create();
            var result = await unitOfWork.SectionRepository.GetListAsync(s => s.VenueId == venueId, cancellationToken).ConfigureAwait(false);
            return Mapper.Map<List<SectionDto>>(result);
        }
    }
}
