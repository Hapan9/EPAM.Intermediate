using EPAM.EF.Entities.Abstraction;

namespace EPAM.EF.Entities
{
    public partial class Section : Entity
    {
        public required string Name { get; set; }

        public Guid VenueId { get; set; }
        public virtual Venue? Venue { get; set; }

        public virtual List<Raw>? Raws { get; set; }
    }
}
