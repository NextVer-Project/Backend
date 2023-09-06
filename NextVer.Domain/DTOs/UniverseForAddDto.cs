using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class UniverseForAddDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string LogoUrl { get; set; }
    }
}