namespace EPAM.Persistence.Entities
{
    public class Seat
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public Guid RawId { get; set; }
        public virtual Raw? Raw { get; set; }

        public virtual List<SeatStatus>? SeatStatuses { get; set; }
        public virtual List<PriceOption>? PriceOptions { get; set; }
        public virtual List<Order>? Orders { get; set; }
    }
}
