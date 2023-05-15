namespace NextVer.Domain.Models
{
    public class ProductionTechnology
    {
        public int ProductionId { get; set; }
        public int TechnologyId { get; set; }

        public virtual ProductionVersion Production { get; set; }
        public virtual Technology Technology { get; set; }

    }
}
