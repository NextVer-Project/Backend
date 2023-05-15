﻿using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public int Runtime { get; set; }
        [Required]
        public string Description { get; set; }
        public string MovieCoverUrl { get; set; }
        public string MovieTrailerUrl { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int AddedByUser { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Notifiction> Notifictions { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<MovieUniverse> MoviesUniverses { get; set; }
        public virtual ICollection<MovieGenre> MoviesGenres { get; set; }
    }
}