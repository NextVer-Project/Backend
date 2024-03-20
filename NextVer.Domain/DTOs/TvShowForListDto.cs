namespace NextVer.Domain.DTOs
{
    public class TvShowForListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Runtime { get; set; }
    }
}