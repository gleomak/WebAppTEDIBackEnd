using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models.DTOs
{
    public class ResidenceReviewDTO
    {
        [Required]
        public string ResidenceId { get; set; }
        [Required]
        public string? Description { get; set; }

        [Required]
        public string? StarRating { get; set; }

        [Required]
        public string? Username { get; set; }
    }
}
