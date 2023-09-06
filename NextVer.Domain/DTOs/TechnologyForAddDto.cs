﻿using System.ComponentModel.DataAnnotations;

namespace NextVer.Domain.DTOs
{
    public class TechnologyForAddDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string LogoUrl { get; set; }
        [Required]
        public int TechnologyTypeId { get; set; }
    }
}