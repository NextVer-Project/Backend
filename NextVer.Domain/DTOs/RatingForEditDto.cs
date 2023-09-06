using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class RatingForEditDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int RatingCategoryId { get; set; }
        [Required]
        public int IdMovieTVSerieGame { get; set; }
        [Required]
        public int ProductionTypeId { get; set; }
        [Required]
        public int RatingValue { get; set; }
    }
}