namespace NextVer.Domain.Models
{
    public class ProductionType
    {
        public int Id { get; set; }
        public string ProductionTypeName { get; set; }
        public string ProductionTypeDescription { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
        public virtual ICollection<Notifiction> Notifictions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ProductionVersion> Productions { get; set; }
    }
}
