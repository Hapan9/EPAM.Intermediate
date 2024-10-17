using EPAM.Persistence.Entities;

namespace EPAM.EF.Entities
{
    public class Seat
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public Guid RawId { get; set; }
        public virtual Raw? Raw { get; set; }
    }
}
