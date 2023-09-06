using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class Genre : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<TvShowGenre> TvShowGenres { get; set; }

    }
}
