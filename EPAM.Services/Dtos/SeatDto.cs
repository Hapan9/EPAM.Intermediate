namespace EPAM.Services.Dtos
{
    public class SeatDto
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public Guid RawId { get; set; }

        public SeatStatusDto? SeatStatusDto { get; set; }

        public PriceOptionDto? PriceOptionDto { get; set; }
    }
}
