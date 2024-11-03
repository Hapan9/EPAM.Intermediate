using EPAM.Services.Dtos;

namespace EPAM.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<EventDto>> GetListAsync(CancellationToken cancellationToken);
    }
}
