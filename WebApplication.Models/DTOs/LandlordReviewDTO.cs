using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Models.DTOs
{
    public class LandlordReviewDTO
    {
        [Required]
        public string? Description { get; set; }

        [Required]
        public string? StarRating { get; set; }

        [Required]
        public string? HostName { get; set; }

        [Required]
        public string? ReviewByUser { get; set; }

    }
}