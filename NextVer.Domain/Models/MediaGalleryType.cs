namespace NextVer.Domain.Models
{
    public class MediaGalleryType
    {
        public int Id { get; set; }
        public string MediaTypeName { get; set; }
        public string MediaTypeDescription { get; set; }

        public virtual ICollection<Gallery> Galleries { get; set; }
    }
}
