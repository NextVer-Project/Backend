using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.Models
{
    public class TvShow
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string TrailerUrl { get; set; }
        public int ProductionTypeId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public int ViewCounter { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<TvShowUniverse> TvShowUniverses { get; set; }
        public virtual ICollection<TvShowGenre> TvShowGenres { get; set; }
    }
}
