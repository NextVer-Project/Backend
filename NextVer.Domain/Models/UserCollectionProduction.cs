namespace NextVer.Domain.Models
{
    public class UserCollectionProduction
    {
        public int UserCollectionId { get; set; }
        public int ProductionVersionId { get; set; }

        public virtual UserCollection UserCollection { get; set; }
        public virtual ProductionVersion ProductionVersion { get; set; }
    }
}
