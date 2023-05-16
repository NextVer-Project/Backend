namespace NextVer.Domain.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<TvShowGenre> TvShowsGenres { get; set; }

    }
}
