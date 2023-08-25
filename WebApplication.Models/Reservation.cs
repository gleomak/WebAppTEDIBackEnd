using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int ResidenceId { get; set; }
        public Residence Residence { get; set; } = null!;
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
