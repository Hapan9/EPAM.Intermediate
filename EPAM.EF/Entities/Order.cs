using EPAM.EF.Entities.Abstraction;

namespace EPAM.EF.Entities
{
    public class Order : Entity
    {
        public Guid CartId { get; set; }

        public Guid EventId { get; set; }
        public virtual Event? Event { get; set; }

        public Guid SeatId { get; set; }
        public virtual Seat? Seat { get; set; }

        public Guid PriceOptionId { get; set; }
        public virtual PriceOption? PriceOption { get; set; }

        public Guid? PaymentId { get; set; }
        public Payment? Payment { get; set; }
    }
}
