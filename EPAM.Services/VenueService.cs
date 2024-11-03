using AutoMapper;
using EPAM.Persistence.UnitOfWork.Interface;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class VenueService : BaseService<VenueService>, IVenueService
    {
        public VenueService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, ILogger<VenueService> logger) : base(unitOfWorkFactory, mapper, logger)
        {
        }

        public async Task<VenueDto> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            using var unitOfWork = UnitOfWorkFactory.Create();
            var venue = await unitOfWork.VenueRepository.GetAsync(v => v.Id == id, cancellationToken).ConfigureAwait(false);
            return Mapper.Map<VenueDto>(venue);
        }

        public async Task<List<VenueDto>> GetListAsync(CancellationToken cancellationToken)
        {
            using var unitOfWork = UnitOfWorkFactory.Create();
            var venues = await unitOfWork.VenueRepository.GetListAsync(cancellationToken).ConfigureAwait(false);
            return Mapper.Map<List<VenueDto>>(venues);
        }
    }
}
