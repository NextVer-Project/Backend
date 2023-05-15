namespace NextVer.Domain.Models
{
    public class RatingCategory
    {
        public int Id { get; set; }
        public string RatingCategoryName { get; set; }
        public string RatingCategoryDescription { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
