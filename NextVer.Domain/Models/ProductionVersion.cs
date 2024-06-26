﻿using NextVer.Infrastructure.Interfaces;

namespace NextVer.Domain.Models
{
    public class ProductionVersion : IEntityWithLinkIds
    {
        public int Id { get; set; }
        public int IdMovieTVSerieGame { get; set; }
        public int ProductionTypeId { get; set; }
        public int ReleasePlaceId { get; set; }
        public string LinkToProductionVersion { get; set; }
        public DateTime ReleasedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public int ViewCounter { get; set; }

        public virtual User User { get; set; }
        public virtual ReleasePlace ReleasePlace { get; set; }
        public virtual ProductionType ProductionType { get; set; }
        public virtual ICollection<UserCollection> UserCollections { get; set; }
        public virtual ICollection<ProductionTechnology> ProductionTechnologies { get; set; }


    }
}
