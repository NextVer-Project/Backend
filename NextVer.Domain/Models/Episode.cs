using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public int TvShowId { get; set; }

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
        public bool IsApproved { get; set; }
        public int AddedByUser { get; set; }
        public int ViewCounter { get; set; }

        public virtual User User { get; set; }
        public virtual TvShow TvShow { get; set; }
    }
}
