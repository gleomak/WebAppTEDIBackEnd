using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models.DTOs
{
    public class ResidenceDTO : Residence 
    {
        public List<string> ImageURL { get; set; } = new List<string>();
        public List<ReservationFromTo> ReservationFromTo { get; set; } = new List<ReservationFromTo>();

        public List<ResidenceReviews> Reviews { get; set; } = new List<ResidenceReviews>();
    }
}
