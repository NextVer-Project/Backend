namespace NextVer.Domain.Models
{
    public class Notifiction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NotifictionTypeId { get; set; }
        public int IdMovieTVSerieGame { get; set; }
        public int ProductionTypeId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual TvShow TvShow { get; set; }
        public virtual ProductionType ProductionType { get; set; }
        public virtual NotifictionType NotifictionType { get; set; }
    }
}

