namespace EPAM.Services.Dtos.Order
{
    public class GetOrderDto
    {
        public Guid Id { get; set; }

        public Guid CartId { get; set; }

        public Guid EventId { get; set; }

        public Guid SeatId { get; set; }

        public Guid PriceOptionId { get; set; }
    }
}
