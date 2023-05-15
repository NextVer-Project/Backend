namespace NextVer.Domain.Models
{
    public class NotifictionType
    {
        public int Id { get; set; }
        public string NotifictionName { get; set; }
        public string NotifictionMessage { get; set; }
        public int UserTypeId { get; set; }

        public virtual UserType UserType { get; set; }
        public virtual ICollection<Notifiction> Notifictions { get; set; }

    }
}
