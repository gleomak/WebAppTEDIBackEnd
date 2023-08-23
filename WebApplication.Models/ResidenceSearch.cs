using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ResidenceSearch : PaginationParams
    {
        public string? city {  get; set; }  
        public string? country { get; set; }    
        public string? neighborhood { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }    
        public int? numOfPeople { get; set; }
    }
}
