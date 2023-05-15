namespace NextVer.Domain.Models
{
    public class UserType
    {
        public int Id { get; set; }
        public string UserTypeName { get; set; }
        public string UserTypeDescription { get; set; }
        public string UserTypeLogoUrl { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Notifiction> Notifictions { get; set; }
    }
}