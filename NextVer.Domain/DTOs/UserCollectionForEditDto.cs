using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class UserCollectionForEditDto
    {
        [Required]
        public int Id { get; set; }
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