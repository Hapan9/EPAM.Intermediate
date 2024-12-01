using EPAM.EF.Entities.Abstraction;

namespace EPAM.EF.Entities
{
    public partial class Venue : Entity
    {
        public required string Name { get; set; }

        public virtual List<Section>? Sections { get; set; }
        public virtual List<Event>? Events { get; set; }
    }
}
