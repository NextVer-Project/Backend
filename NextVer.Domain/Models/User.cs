using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Encrypted]
        public string FirstName { get; set; }
        [Encrypted]
        public string LastName { get; set; }
        [Encrypted]
        public string Country { get; set; }
        [Encrypted]
        public string City { get; set; }
        [Required, Encrypted, EmailAddress]
        public string Email { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public int UserTypeId { get; set; }
        public string? UserLogoUrl { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public bool NotificationsAgreement { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Encrypted]
        public string ConfirmationToken { get; set; }
        public DateTime ConfirmationTokenGeneratedTime { get; set; }
        public int Points { get; set; }

        public virtual UserType UserType { get; set; }
        public virtual ICollection<ProductionVersion> Productions { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<TvShow> TvShows { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<UserCollection> UserCollections { get; set; }
    }
}