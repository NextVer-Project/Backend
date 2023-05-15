namespace NextVer.Domain.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string GenreName { get; set; }
        public string GenreDescription { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<TvShowGenre> TvShowsGenres { get; set; }

    }
}
