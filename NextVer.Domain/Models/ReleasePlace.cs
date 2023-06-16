namespace NextVer.Domain.Models
{
    public class ReleasePlace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public string LinkUrl { get; set; }
        public int ReleasePlaceTypeId { get; set; }

        public virtual ReleasePlaceType ReleasePlaceType { get; set; }
        public virtual ICollection<ProductionVersion> ProductionVersions { get; set; }
    }
}
