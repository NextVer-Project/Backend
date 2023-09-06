using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class Comment : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int IdMovieTVSerieGame { get; set; }
        public int ProductionTypeId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual ProductionType ProductionType { get; set; }
    }
}
