namespace EPAM.Services.Dtos
{
    public class EventDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public DateTime? Date { get; set; }

        public Guid VenueId { get; set; }
    }
}
