namespace NextVer.Domain.Models
{
    public class Technology
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public int TechnologyTypeId { get; set; }

        public virtual TechnologyType TechnologyType { get; set; }

    }
}
