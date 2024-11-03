using AutoMapper;
using EPAM.Persistence.UnitOfWork.Interface;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class EventService : BaseService<EventService>, IEventService
    {
        public EventService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, ILogger<EventService> logger) : base(unitOfWorkFactory, mapper, logger)
        {
        }

        public async Task<List<EventDto>> GetListAsync(CancellationToken cancellationToken)
        {
            using var unitOfWork = UnitOfWorkFactory.Create();
            var result = await unitOfWork.EventRepository.GetListAsync(cancellationToken).ConfigureAwait(false);
            return Mapper.Map<List<EventDto>>(result);
        }
    }
}
