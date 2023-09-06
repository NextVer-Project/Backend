using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class TvShowForAddDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public int Runtime { get; set; }
        [Required]
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string TrailerUrl { get; set; }
        public List<int> GenreIds { get; set; }
        public List<int> UniverseIds { get; set; }
    }
}