namespace NextVer.Domain.Models
{
    public class TvShowGenre
    {
        public int TvShowId { get; set; }
        public int GenreId { get; set; }

        public virtual TvShow TvShow { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
