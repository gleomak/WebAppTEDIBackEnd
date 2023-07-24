using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models.DTOs
{
    public  class RegisterDTO : LoginDTO
    {
        public string Email { get; set; }
    }
}
