namespace NextVer.Domain.Models
{
    public class MovieUniverse
    {
        public int MovieId { get; set; }
        public int UniverseId { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Universe Universe { get; set; }
    }
}
