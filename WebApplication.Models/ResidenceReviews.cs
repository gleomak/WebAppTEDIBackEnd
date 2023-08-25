using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ResidenceReviews
    {
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public double? StarRating { get; set; }
        
        public int ResidenceId { get; set; }
        public Residence Residence { get; set; } = null!;

        //public int UserId { get; set; }
        //public User User { get; set; } = null!;

    }
}
