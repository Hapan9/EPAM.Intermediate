namespace EPAM.EF.Models
{
    public class SeatBooked
    {
        public required string EventName { get; set; }

        public required string VenueName { get; set; }

        public required string SectionName { get; set; }

        public int RawNumber { get; set; }

        public int SeatNumber { get; set; }

        public decimal Price { get; set; }
    }
}
