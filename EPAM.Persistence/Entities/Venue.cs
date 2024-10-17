namespace EPAM.Persistence.Entities
{
    public class Venue
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public virtual List<Section>? Sections { get; set; }
        public virtual List<Event>? Events { get; set; }
    }
}
