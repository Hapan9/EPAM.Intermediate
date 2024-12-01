using EPAM.EF.Entities.Abstraction;

namespace EPAM.EF.Entities
{
    public class PriceOption : Entity
    {
        public decimal Price { get; set; }

        public Guid EventId { get; set; }
        public virtual Event? Event { get; set; }

        public Guid SeatId { get; set; }
        public virtual Seat? Seat { get; set; }

        public virtual Order? Order { get; set; }
    }
}
