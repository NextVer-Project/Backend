using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class TechnologiesForProductionVersionDto
    {
        [Required]
        public int ProductionVersionId { get; set; }
        [Required]
        public IEnumerable<TechnologyForEditDto> ProductionTechnologies { get; set; }
    }
}