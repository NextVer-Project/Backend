using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class MediaGalleryTypeForAddDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}