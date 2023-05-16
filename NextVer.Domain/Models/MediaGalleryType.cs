namespace NextVer.Domain.Models
{
    public class MediaGalleryType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Gallery> Galleries { get; set; }
    }
}
