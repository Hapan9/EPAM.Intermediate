namespace EPAM.Persistence.Entities
{
    public class Event
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public DateTime? Date { get; set; }

        public Guid VenueId { get; set; }
        public virtual Venue? Venue { get; set; }
    }
}
