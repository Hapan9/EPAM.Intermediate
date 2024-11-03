using EPAM.Services.Dtos;

namespace EPAM.Services.Interfaces
{
    public interface ISeatService
    {
        Task<List<SeatDto>> GetByEventIdAndSectionIdAsync(Guid eventId, Guid sectionId, CancellationToken cancellationToken);
    }
}
