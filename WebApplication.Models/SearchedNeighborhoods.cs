using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class SearchedNeighborhoods
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Neighborhood { get; set; }
        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
