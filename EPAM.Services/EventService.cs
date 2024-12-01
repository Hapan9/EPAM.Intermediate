using AutoMapper;
using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class EventService : BaseService<EventService>, IEventService
    {
        public EventService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EventService> logger) : base(unitOfWork, mapper, logger)
        {
        }

        public async Task<List<EventDto>> GetListAsync(CancellationToken cancellationToken)
        {
            var result = await UnitOfWork.EventRepository.GetListAsync(cancellationToken).ConfigureAwait(false);
            return Mapper.Map<List<EventDto>>(result);
        }
    }
}
