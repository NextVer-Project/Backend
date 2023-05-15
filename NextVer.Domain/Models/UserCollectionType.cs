namespace NextVer.Domain.Models
{
    public class UserCollectionType
    {
        public int Id { get; set; }
        public string UserCollectionTypeName { get; set; }
        public string UserCollectionTypeDescription { get; set; }

        public virtual ICollection<UserCollection> UserCollections { get; set; }
    }
}