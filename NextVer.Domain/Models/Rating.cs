using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RatingCategoryId { get; set; }
        public int IdMovieTVSerieGame { get; set; }
        public int ProductionTypeId { get; set; }
        [Range(typeof(int), "0.1", "10")]
        public int RatingValue { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual TvShow TvShow { get; set; }
        public virtual ProductionType ProductionType { get; set; }
        public virtual RatingCategory RatingCategory { get; set; }
    }
}
