using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class ReleasePlaceType : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ReleasePlace> ReleasePlaces { get; set; }
    }
}
