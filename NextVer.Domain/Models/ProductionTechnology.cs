namespace NextVer.Domain.Models
{
    public class ProductionTechnology
    {
        public int ProductionVersionId { get; set; }
        public int TechnologyId { get; set; }

        public virtual ProductionVersion ProductionVersion { get; set; }
        public virtual Technology Technology { get; set; }

    }
}
