﻿using System.ComponentModel.DataAnnotations;

namespace WebApiLU2.Models
{
    public class Environment2D
    {
        [Key]
        public Guid Id { get; set; } // Primaire sleutel

        [Required]
        public required string Name { get; set; } // Naam van de wereld
        public int MaxHeight { get; set; } // Hoogte van de wereld
        public int MaxLength { get; set; } // Lengte van de wereld
        public Guid UserId { get; set; } // Buitenlandse sleutel naar ApplicationUser
    }

}
