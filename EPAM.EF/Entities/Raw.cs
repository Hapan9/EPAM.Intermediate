using EPAM.EF.Entities.Abstraction;

namespace EPAM.EF.Entities
{
    public partial class Raw : Entity
    {
        public int Number { get; set; }

        public Guid SectionId { get; set; }
        public virtual Section? Section { get; set; }

        public virtual List<Seat>? Seats { get; set; }
    }
}
