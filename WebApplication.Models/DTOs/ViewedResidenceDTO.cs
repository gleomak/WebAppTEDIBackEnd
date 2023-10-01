using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models.DTOs
{
    public class ViewedResidenceDTO
    {
        public string? UserId { get; set; }
        public int ResidenceId { get; set; }
    }
}
