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
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string? URL { get; set; }
        [Required]
        public string? PublicId { get; set; }
        public int ResidenceId { get; set; }
        public Residence Residence { get; set; } = null!;
    }
}
