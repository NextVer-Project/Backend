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
        public string EpisodeTitle { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(typeof(TimeSpan), "00:00:01", "02:59:59")]
        public TimeSpan EpisodeDuration { get; set; }

        [Required]
        public DateTime EpisodeReleaseDate { get; set; }
        public int AddeBy { get; set; }

        public virtual User User { get; set; }
        public virtual TvShow TvShow { get; set; }
    }
}
