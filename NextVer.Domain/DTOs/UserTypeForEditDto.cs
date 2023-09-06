using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class UserTypeForEditDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string LogoUrl { get; set; }

    }
}