using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class UserCollectionType : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserCollection> UserCollections { get; set; }
    }
}