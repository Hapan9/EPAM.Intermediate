using AutoMapper;
using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class VenueService : BaseService<VenueService>, IVenueService
    {
        public VenueService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VenueService> logger) : base(unitOfWork, mapper, logger)
        {
        }

        public async Task<VenueDto> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var venue = await UnitOfWork.VenueRepository.GetAsync(v => v.Id == id, cancellationToken).ConfigureAwait(false);
            return Mapper.Map<VenueDto>(venue);
        }

        public async Task<List<VenueDto>> GetListAsync(CancellationToken cancellationToken)
        {
            var venues = await UnitOfWork.VenueRepository.GetListAsync(cancellationToken).ConfigureAwait(false);
            return Mapper.Map<List<VenueDto>>(venues);
        }
    }
}
