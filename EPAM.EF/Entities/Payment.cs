using EPAM.EF.Entities.Abstraction;
using EPAM.EF.Entities.Enums;

namespace EPAM.EF.Entities
{
    public class Payment : Entity
    {
        public PaymentStatus Status { get; set; }

        public virtual List<Order>? Orders { get; set; }
    }
}
