using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class Notification : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NotificationTypeId { get; set; }
        public int IdMovieTVSerieGame { get; set; }
        public int ProductionTypeId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual ProductionType ProductionType { get; set; }
        public virtual NotificationType NotificationType { get; set; }
    }
}

