using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class Gallery : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public int IdMovieTVSerieGame { get; set; }
        public int ProductionTypeId { get; set; }
        public int MediaGalleryTypeId { get; set; }
        public string UrlLink { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ProductionType ProductionType { get; set; }
        public virtual MediaGalleryType MediaGalleryType { get; set; }
    }
}
