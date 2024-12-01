using EPAM.EF.Entities.Abstraction;

namespace EPAM.EF.Entities
{
    public partial class Event : Entity
    {
        public required string Name { get; set; }

        public DateTime? Date { get; set; }

        public Guid VenueId { get; set; }
        public virtual Venue? Venue { get; set; }

        public virtual List<SeatStatus>? SeatStatuses { get; set; }
        public virtual List<PriceOption>? PriceOptions { get; set; }
        public virtual List<Order>? Orders { get; set; }
    }
}
