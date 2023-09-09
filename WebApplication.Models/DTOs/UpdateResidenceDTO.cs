using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models.DTOs
{
    public class UpdateResidenceDTO
    {
        [Required]
        public List<string> ImagesToDelete { get; set; }
        [Required]
        public List<IFormFile> FilesToAdd { get; set; }
    }
}
