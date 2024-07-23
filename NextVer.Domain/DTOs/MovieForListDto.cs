namespace NextVer.Domain.DTOs
{
    public class MovieForListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string TrailerUrl { get; set; }
        public string Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}