using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string PictureURL { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles {get; set;}

        public bool RoleAuthorized { get; set; }
    }
}
