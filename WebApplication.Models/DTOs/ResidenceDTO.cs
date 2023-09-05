using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models.DTOs
{
    public class ResidenceDTO : Residence 
    {
        public List<ReservationFromTo> ReservationFromTo { get; set; } = new List<ReservationFromTo>();
    }
}
