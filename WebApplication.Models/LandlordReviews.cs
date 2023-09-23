using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class LandlordReviews
    {
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public double StarRating { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; } = null!;

        [Required]
        public string ReviewByUser { get; set; }
    }
}
