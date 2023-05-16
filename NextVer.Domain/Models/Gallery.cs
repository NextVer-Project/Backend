namespace NextVer.Domain.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        public int IdMovieTVSerieGame { get; set; }
        public int ProductionTypeId { get; set; }
        public int MediaTypeId { get; set; }
        public string UrlLink { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual TvShow TvShow { get; set; }
        public virtual ProductionType ProductionType { get; set; }
        public virtual MediaGalleryType MediaGalleryType { get; set; }
    }
}
