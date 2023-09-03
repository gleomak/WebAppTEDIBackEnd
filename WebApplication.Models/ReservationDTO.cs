using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ReservationDTO
    {
        public int ResidenceId { get; set; }
        public DateTime From { get; set; } 
        public DateTime To { get; set; }
        public string Username { get; set; }
    }
}
