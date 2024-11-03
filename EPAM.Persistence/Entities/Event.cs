namespace EPAM.Persistence.Entities
{
    public class Event
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public DateTime? Date { get; set; }

        public Guid VenueId { get; set; }
        public virtual Venue? Venue { get; set; }

        public virtual List<SeatStatus>? SeatStatuses { get; set; }
        public virtual List<PriceOption>? PriceOptions { get; set; }
        public virtual List<Order>? Orders { get; set; }
    }
}
