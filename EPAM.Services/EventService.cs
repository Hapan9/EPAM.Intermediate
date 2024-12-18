using AutoMapper;
using EPAM.Cache.Enums;
using EPAM.Cache.Interfaces;
using EPAM.EF.UnitOfWork.Interfaces;
using EPAM.Services.Abstraction;
using EPAM.Services.Dtos;
using EPAM.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services
{
    public sealed class EventService : BaseService<EventService>, IEventService
    {
        const CacheTypes CacheType = CacheTypes.DistributedCache;
        private readonly ISystemCache _systemCache;

        public EventService(ISystemCache systemCache, IUnitOfWork unitOfWork, IMapper mapper, ILogger<EventService> logger) : base(unitOfWork, mapper, logger)
        {
            _systemCache = systemCache;
        }

        public async Task<List<EventDto>> GetListAsync(CancellationToken cancellationToken)
        {
            var cacheResult = await _systemCache.GetCache(CacheType).GetAsync<List<EventDto>>("EventsList", cancellationToken);

            if (cacheResult != null) return cacheResult;

            var result = await UnitOfWork.EventRepository.GetListAsync(cancellationToken).ConfigureAwait(false);

            await _systemCache.GetCache(CacheType).SetAsync("EventsList", result, cancellationToken);

            return Mapper.Map<List<EventDto>>(result);
        }
    }
}
