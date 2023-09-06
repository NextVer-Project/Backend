using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class UserCollectionForAddDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int UserCollectionTypeId { get; set; }
        [Required]
        public List<int> ProductionVersionIds { get; set; }
    }
}