using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class MovieForEditDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public int Runtime { get; set; }
        [Required]
        public string CoverUrl { get; set; }
        [Required]
        public string TrailerUrl { get; set; }
        [Required]
        public bool IsApproved { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        public List<int> GenreIds { get; set; }
        public List<int> UniverseIds { get; set; }
    }
}
