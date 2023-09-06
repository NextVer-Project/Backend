using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class CommentForAddDto
    {
        [Required]
        public int IdMovieTVSerieGame { get; set; }
        [Required]
        public int ProductionTypeId { get; set; }
        [Required]
        public string CommentText { get; set; }
    }
}