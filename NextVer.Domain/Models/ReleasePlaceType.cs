namespace NextVer.Domain.Models
{
    public class ReleasePlaceType
    {
        public int Id { get; set; }
        public string ReleasePlaceTypeName { get; set; }
        public string ReleasePlaceTypeDescription { get; set; }

        public virtual ICollection<ReleasePlace> ReleasePlaces { get; set; }
    }
}
