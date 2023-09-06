using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class NotificationType : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public int UserTypeId { get; set; }

        public virtual UserType UserType { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

    }
}
