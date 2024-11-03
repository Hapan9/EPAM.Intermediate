namespace EPAM.Persistence.Entities
{
    public class SeatStatus
    {
        public Guid Id { get; set; }

        public Enums.SeatStatus Status { get; set; }

        public DateTime? LastStatusChangeDt { get; set; }

        public Guid EventId { get; set; }
        public virtual Event? Event { get; set; }

        public Guid SeatId { get; set; }
        public virtual Seat? Seat { get; set; }
    }
}
