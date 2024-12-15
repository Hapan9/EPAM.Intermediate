using EPAM.EF.Entities.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace EPAM.EF.Entities
{
    public class SeatStatus : Entity
    {
        public Enums.SeatStatus Status { get; set; }

        public DateTime? LastStatusChangeDt { get; set; }

        public Guid EventId { get; set; }
        public virtual Event? Event { get; set; }

        public Guid SeatId { get; set; }
        public virtual Seat? Seat { get; set; }

        [ConcurrencyCheck]
        public Guid Version { get; set; }
    }
}
