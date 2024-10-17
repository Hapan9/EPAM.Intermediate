namespace EPAM.Persistence.Entities
{
    public class Section
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public Guid VenueId { get; set; }
        public virtual Venue? Venue { get; set; }
        public virtual List<Raw>? Raws { get; set; }
    }
}
