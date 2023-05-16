namespace NextVer.Domain.Models
{
    public class Universe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<TvShow> TvShows { get; set; }
    }
}
