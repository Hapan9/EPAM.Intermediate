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
    public class SeatService : BaseService<SeatService>, ISeatService
    {
        const CacheTypes CacheType = CacheTypes.DistributedCache;
        private readonly ISystemCache _systemCache;

        public SeatService(ISystemCache systemCache, IUnitOfWork unitOfWork, IMapper mapper, ILogger<SeatService> logger) : base(unitOfWork, mapper, logger)
        {
            _systemCache = systemCache;
        }

        public async Task<List<SeatDto>> GetByEventIdAndSectionIdAsync(Guid eventId, Guid sectionId, CancellationToken cancellationToken)
        {

            var cacheResult = await _systemCache.GetCache(CacheType).GetAsync<List<SeatDto>>($"Event-{eventId}-Section-{sectionId}-seats", cancellationToken);

            if (cacheResult != null) return cacheResult;

            var seats = await UnitOfWork.SeatRepository.GetListAsync(s => s.Raw!.SectionId == sectionId, cancellationToken).ConfigureAwait(false);

            var seatsStatuses = await UnitOfWork.SeatStatusRepository.GetListAsync(s => s.EventId == eventId && s.Seat!.Raw!.SectionId == sectionId, cancellationToken).ConfigureAwait(false);
            var priceOptions = await UnitOfWork.PriceOptionRepository.GetListAsync(p => p.EventId == eventId && p.Seat!.Raw!.SectionId == sectionId, cancellationToken).ConfigureAwait(false);

            var result = Mapper.Map<List<SeatDto>>(seats, opt =>
            {
                opt.AfterMap((_, dest) =>
                {
                    foreach (var seat in dest)
                    {
                        var seatsStatus = seatsStatuses.Where(s => s.SeatId == seat.Id).FirstOrDefault();
                        var priceOption = priceOptions.Where(p => p.SeatId == seat.Id).FirstOrDefault();

                        seat.SeatStatusDto = Mapper.Map<SeatStatusDto?>(seatsStatus);
                        seat.PriceOptionDto = Mapper.Map<PriceOptionDto?>(priceOption);
                    }
                });
            });

            await _systemCache.GetCache(CacheType).SetAsync($"Event-{eventId}-Section-{sectionId}-seats", result, cancellationToken);

            return result;
        }
    }
}
