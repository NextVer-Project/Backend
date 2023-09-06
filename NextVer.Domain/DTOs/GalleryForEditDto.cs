using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class GalleryForEditDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdMovieTVSerieGame { get; set; }
        [Required]
        public int ProductionTypeId { get; set; }
        [Required]
        public int MediaGalleryTypeId { get; set; }
        [Required]
        public string UrlLink { get; set; }
    }
}