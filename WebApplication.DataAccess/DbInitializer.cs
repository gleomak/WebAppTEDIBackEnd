using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Data;
using WebApp.Models;

namespace WebApp.DataAccess
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext db, UserManager<User> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "bob",
                    Email = "bob@test.com",
                    FirstName = "Bobber",
                    LastName = "Bobbings",
                    StreetAddress = "Bob 2",
                    PhoneNumber = "697342472"

                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
                var admin = new User
                {
                    UserName = "Admin",
                    FirstName = "Admin",
                    LastName = "Adminakis",
                    StreetAddress = "Adminaki 3",
                    Email = "admin@test.com"
                };
                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] {"Member", "Admin"});
            }
        }
    }
}
