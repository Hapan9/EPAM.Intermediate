using EPAM.EF.Entities.Abstraction;

namespace EPAM.EF.Entities
{
    public partial class Seat : Entity
    {
        public int Number { get; set; }

        public Guid RawId { get; set; }
        public virtual Raw? Raw { get; set; }

        public virtual List<SeatStatus>? SeatStatuses { get; set; }
        public virtual List<PriceOption>? PriceOptions { get; set; }
        public virtual List<Order>? Orders { get; set; }
    }
}
