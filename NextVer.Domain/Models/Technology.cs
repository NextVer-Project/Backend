namespace NextVer.Domain.Models
{
    public class Technology
    {
        public int Id { get; set; }
        public string TechnologyName { get; set; }
        public string TechnologyDescription { get; set; }
        public string TechnologyLogoUrl { get; set; }
        public string TechnologyTypeId { get; set; }

        public virtual TechnologyType TechnologyType { get; set; }

    }
}
