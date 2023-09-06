using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class NotificationForAddDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int NotificationTypeId { get; set; }
        [Required]
        public int IdMovieTVSerieGame { get; set; }
        [Required]
        public int ProductionTypeId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}