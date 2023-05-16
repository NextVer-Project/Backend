namespace NextVer.Domain.Models
{
    public class TvShowUniverse
    {
        public int TvShowId { get; set; }
        public int UniverseId { get; set; }

        public virtual TvShow TvShow { get; set; }
        public virtual Universe Universe { get; set; }
    }
}