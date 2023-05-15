namespace NextVer.Domain.Models
{
    public class Universe
    {
        public int Id { get; set; }
        public string UniverseName { get; set; }
        public string UniverseDescription { get; set; }
        public string UniverseLogoUrl { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<TvShow> TvShows { get; set; }
    }
}
