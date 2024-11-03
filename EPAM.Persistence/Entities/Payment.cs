using EPAM.Persistence.Entities.Enums;

namespace EPAM.Persistence.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }

        public PaymentStatus Status { get; set; }

        public virtual List<Order>? Orders { get; set; }
    }
}
