using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models.DTOs
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string? ResidenceTitle { get; set; }
        public string? Message { get; set; }
        public string? SenderUsername { get; set; }
        public string? SenderImageURL { get; set; }

    }
}
