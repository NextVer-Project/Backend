using NextVer.Domain.Models;

namespace NextVer.Domain.DTOs
{
    public class ProductionVersionDetailsDto
    {
        public int Id { get; set; }
        public int IdMovieTVSerieGame { get; set; }
        public int ProductionTypeId { get; set; }
        public int ReleasePlaceId { get; set; }
        public string LinkToProductionVersion { get; set; }
        public DateTime ReleasedDate { get; set; }

        public List<Technology> Technologies { get; set; }
    }
}