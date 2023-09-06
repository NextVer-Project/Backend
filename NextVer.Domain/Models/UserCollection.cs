using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class UserCollection : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int UserCollectionTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual UserCollectionType UserCollectionType { get; set; }
        public virtual ICollection<UserCollectionProduction> UserCollectionProductions { get; set; }
    }
}
