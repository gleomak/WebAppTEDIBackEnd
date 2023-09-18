using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? MessageBody { get; set; }

        [Required]
        public string? UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public string? SenderUsername { get; set; }
        

    }
}
