using EPAM.EF.Entities;

namespace EPAM.Persistence.Entities
{
    public class Raw
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public Guid SectionId { get; set; }
        public virtual Section? Section { get; set; }
        public virtual List<Seat>? Seats { get; set; }
    }
}
