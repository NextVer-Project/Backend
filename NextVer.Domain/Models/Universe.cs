namespace NextVer.Domain.Models
{
    public class Universe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }

        public virtual ICollection<MovieUniverse> MovieUniverses { get; set; }
        public virtual ICollection<TvShowUniverse> TvShowUniverses { get; set; }
    }
}
