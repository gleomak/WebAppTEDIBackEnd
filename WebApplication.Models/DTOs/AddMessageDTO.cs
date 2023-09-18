using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models.DTOs
{
    public class AddMessageDTO
    {
        public string? MessageBody { get; set; }
        public string? RecipientUsername { get; set; }
    }
}
