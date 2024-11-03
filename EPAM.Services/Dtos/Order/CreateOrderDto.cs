namespace EPAM.Services.Dtos.Order
{
    public class CreateOrderDto
    {
        public Guid EventId { get; set; }

        public Guid SeatId { get; set; }

        public Guid PriceOptionId { get; set; }
    }
}
