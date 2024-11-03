using EPAM.Services.Dtos;

namespace EPAM.Services.Interfaces
{
    public interface IVenueService
    {
        public Task<VenueDto> GetAsync(Guid id, CancellationToken cancellationToken);

        public Task<List<VenueDto>> GetListAsync(CancellationToken cancellationToken);
    }
}
