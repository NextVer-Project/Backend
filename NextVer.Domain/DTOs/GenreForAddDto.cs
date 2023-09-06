using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class GenreForAddDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}