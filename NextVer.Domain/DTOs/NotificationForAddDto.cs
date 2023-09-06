using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class NotificationTypeForAddDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public int UserTypeId { get; set; }
    }
}