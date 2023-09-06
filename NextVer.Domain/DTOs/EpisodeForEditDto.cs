using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class EpisodeForEditDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int SeasonNumber { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }
    }
}