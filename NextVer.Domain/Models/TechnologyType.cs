namespace NextVer.Domain.Models
{
    public class TechnologyType
    {
        public int Id { get; set; }
        public string TechnologyTypeName { get; set; }
        public string TechnologyTypeDescription { get; set; }

        public virtual ICollection<Technology> Technologies { get; set; }
    }
}
