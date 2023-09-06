using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class ProductionVersionForAddDto
    {
        [Required]
        public int IdMovieTVSerieGame { get; set; }
        [Required]
        public int ProductionTypeId { get; set; }
        [Required]
        public int ReleasePlaceId { get; set; }
        [Required]
        public string LinkToProductionVersion { get; set; }
        [Required]
        public DateTime ReleasedDate { get; set; }

        public List<int> TechnologyIds { get; set; }
    }
}