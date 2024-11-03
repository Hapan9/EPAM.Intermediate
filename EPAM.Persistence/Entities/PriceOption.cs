namespace EPAM.Persistence.Entities
{
    public class PriceOption
    {
        public Guid Id { get; set; }

        public decimal Price { get; set; }

        public Guid EventId { get; set; }
        public virtual Event? Event { get; set; }

        public Guid SeatId { get; set; }
        public virtual Seat? Seat { get; set; }

        public virtual Order? Order { get; set; }
    }
}
