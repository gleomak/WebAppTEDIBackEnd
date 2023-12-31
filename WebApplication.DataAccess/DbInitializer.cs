﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebApp.DataAccess
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext db, UserManager<User> userManager, IUnitOfWork _unitOfWork)
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
                    PhoneNumber = "697342472",
                    RoleAuthorized = true,
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");

                var admin = new User
                {
                    UserName = "Admin",
                    FirstName = "Admin",
                    LastName = "Adminakis",
                    PhoneNumber = "696969696",
                    StreetAddress = "Adminaki 3",
                    Email = "admin@test.com",
                    RoleAuthorized = true
                
                };
                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] { "Member", "Admin" });

                var host = new User
                {
                    UserName = "Host",
                    Email = "host@test.com",
                    FirstName = "Hosterer",
                    LastName = "Hostings",
                    StreetAddress = "Host 2",
                    PhoneNumber = "692345432",
                    RoleAuthorized = false,
                   
                };
                await userManager.CreateAsync(host, "Pa$$w0rd");
                await userManager.AddToRoleAsync(host, "Host");
            }

            if (db.Residences.Any()) return;

            //Residence residence = new Residence();
            //residence.NumOfBeds = 1;
            //residence.Title = "Villa X";
            //residence.Address = "XXX 6";
            //residence.Latitude = 2;
            //residence.Longitude = 3;
            //residence.City = "Athens";
            //residence.Country = "Greece";
            //residence.ResidentCapacity = 5;
            //residence.Neighborhood = "Nea Smyrni";
            //residence.NumOfBathrooms = 1;
            //residence.NumOfBedrooms = 2;
            //residence.ResidenceType = "Treehouse";
            //residence.LivingRoom = false;
            //residence.SquareMeters = 100;
            //residence.Description = "Description";
            //residence.Smoking = false;
            //residence.Pets = true;
            //residence.Events = true;
            //residence.Internet = false;
            //residence.Aircondition = true;
            //residence.Kitchen = true;
            //residence.ParkingSpot = true;
            //residence.Tv = false;
            //residence.MinDaysForReservation = 3;
            //residence.UserId = "a3128662-aae2-46d3-92c0-7c6eee189e66";
            ////_unitOfWork.Residence.Add(residence);
            ////_unitOfWork.Save();
            //db.Residences.Add(residence);

            //Residence residence2 = new Residence();
            //residence2.NumOfBeds = 1;
            //residence2.Title = "Villa B";
            //residence2.Address = "XXX 6";
            //residence2.Latitude = 2;
            //residence2.Longitude = 3;
            //residence2.City = "Athens";
            //residence2.Country = "Greece";
            //residence2.ResidentCapacity = 8;
            //residence2.Neighborhood = "Penteli";
            //residence2.NumOfBathrooms = 1;
            //residence2.NumOfBedrooms = 2;
            //residence2.ResidenceType = "Treehouse";
            //residence2.LivingRoom = false;
            //residence2.SquareMeters = 100;
            //residence2.Description = "Description";
            //residence2.Smoking = false;
            //residence2.Pets = true;
            //residence2.Events = true;
            //residence2.Internet = false;
            //residence2.Aircondition = true;
            //residence2.Kitchen = true;
            //residence2.ParkingSpot = true;
            //residence2.Tv = false;
            //residence2.MinDaysForReservation = 4;
            //residence2.UserId = "a3128662-aae2-46d3-92c0-7c6eee189e66";

            ////_unitOfWork.Residence.Add(residence2);
            ////_unitOfWork.Save();
            //db.Residences.Add(residence2);

            //Residence residence3 = new Residence();
            //residence3.NumOfBeds = 1;
            //residence3.Title = "Villa V";
            //residence3.Address = "XXX 6";
            //residence3.Latitude = 2;
            //residence3.Longitude = 3;
            //residence3.City = "Patra";
            //residence3.Country = "Greece";
            //residence3.ResidentCapacity = 3;
            //residence3.Neighborhood = "Agia";
            //residence3.NumOfBathrooms = 1;
            //residence3.NumOfBedrooms = 2;
            //residence3.ResidenceType = "Treehouse";
            //residence3.LivingRoom = false;
            //residence3.SquareMeters = 100;
            //residence3.Description = "Description";
            //residence3.Smoking = false;
            //residence3.Pets = true;
            //residence3.Events = true;
            //residence3.Internet = false;
            //residence3.Aircondition = true;
            //residence3.Kitchen = true;
            //residence3.ParkingSpot = true;
            //residence3.Tv = false;
            //residence3.MinDaysForReservation = 1;
            //residence3.UserId = "a3128662-aae2-46d3-92c0-7c6eee189e66";

            ////_unitOfWork.Residence.Add(residence3);
            ////_unitOfWork.Save();
            //db.Residences.Add(residence3);

            //Residence residence4 = new Residence();
            //residence4.NumOfBeds = 1;
            //residence4.Title = "Villa Z";
            //residence4.Address = "XXX 6";
            //residence4.Latitude = 2;
            //residence4.Longitude = 3;
            //residence4.City = "Ioannina";
            //residence4.Country = "Greece";
            //residence4.ResidentCapacity = 3;
            //residence4.Neighborhood = "Agia";
            //residence4.NumOfBathrooms = 1;
            //residence4.NumOfBedrooms = 2;
            //residence4.ResidenceType = "Treehouse";
            //residence4.LivingRoom = false;
            //residence4.SquareMeters = 100;
            //residence4.Description = "Description";
            //residence4.Smoking = false;
            //residence4.Pets = true;
            //residence4.Events = true;
            //residence4.Internet = false;
            //residence4.Aircondition = true;
            //residence4.Kitchen = true;
            //residence4.ParkingSpot = true;
            //residence4.Tv = false;
            //residence4.MinDaysForReservation = 1;
            //residence4.UserId = "a3128662-aae2-46d3-92c0-7c6eee189e66";

            ////_unitOfWork.Residence.Add(residence4);
            ////_unitOfWork.Save();
            //db.Residences.Add(residence4);
            await db.SaveChangesAsync();
        }
    }
}
