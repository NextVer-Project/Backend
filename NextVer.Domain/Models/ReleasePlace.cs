namespace NextVer.Domain.Models
{
    public class ReleasePlace
    {
        public int Id { get; set; }
        public string ReleasePlaceName { get; set; }
        public string ReleasePlaceDescription { get; set; }
        public string ReleasePlaceLogoUrl { get; set; }
        public string ReleasePlaceLinkUrl { get; set; }

        public virtual ICollection<ProductionVersion> ProductionVersions { get; set; }
    }
}
