namespace EPAM.Services.Dtos
{
    public class SeatStatusDto
    {
        public Guid Id { get; set; }

        public required string Status { get; set; }
    }
}
