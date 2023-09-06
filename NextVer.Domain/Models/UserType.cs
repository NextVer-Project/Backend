using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class UserType : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<NotificationType> NotificationTypes { get; set; }
    }
}