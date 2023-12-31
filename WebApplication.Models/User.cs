﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? StreetAddress { get; set; }

        public string? PictureURL { get; set; }

        public string? PublicId { get; set; }
        public bool RoleAuthorized { get; set; }

        public ICollection<Message> Messages { get;} = new List<Message>();
        public ICollection<ViewedResidences> ViewedResidences { get; } = new List<ViewedResidences>();
        public ICollection<SearchedNeighborhoods> SearchedNeighborhoods { get; } = new List<SearchedNeighborhoods>();
    }
}
