using EPAM.Services.Dtos;

namespace EPAM.Services.Interfaces
{
    public interface ISectionService
    {
        Task<List<SectionDto>> GetSectionsByVenueId(Guid venueId, CancellationToken cancellationToken);
    }
}
