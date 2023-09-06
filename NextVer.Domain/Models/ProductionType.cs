using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class ProductionType : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ProductionVersion> Productions { get; set; }
    }
}
