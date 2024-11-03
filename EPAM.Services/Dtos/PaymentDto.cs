namespace EPAM.Services.Dtos
{
    public class PaymentDto
    {
        public Guid Id { get; set; }

        public required string Status { get; set; }

        public decimal Price { get; set; }
    }
}
