namespace NextVer.Domain.Models
{
    public class UserCollectionProduction
    {
        public int UserCollectionId { get; set; }
        public int ProductionId { get; set; }

        public virtual UserCollection UserCollection { get; set; }
        public virtual ProductionVersion Production { get; set; }
    }
}
