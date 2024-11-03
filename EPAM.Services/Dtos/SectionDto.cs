namespace EPAM.Services.Dtos
{
    public class SectionDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public Guid VenueId { get; set; }
    }
}
