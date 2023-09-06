using NextVer.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.Models
{
    public class Movie : IEntityWithLinkIds
    {
        public int Id { get; set; }
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
        public bool IsApproved { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public int ViewCounter { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<MovieUniverse> MovieUniverses { get; set; }
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
