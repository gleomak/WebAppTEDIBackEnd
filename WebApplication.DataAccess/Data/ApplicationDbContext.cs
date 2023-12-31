﻿using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Residence> Residences { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<LandlordReviews> LandlordReviews { get; set; }
        public DbSet<ResidenceReviews> ResidenceReviews { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ViewedResidences> ViewedResidences { get; set; }
        public DbSet<SearchedNeighborhoods> SearchedNeighborhoods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole { Name = "Member", NormalizedName = "MEMBER" },
                    new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                    new IdentityRole { Name = "Host", NormalizedName = "HOST" });
            builder.Entity<ViewedResidences>()
                .HasOne(vr => vr.User)
                .WithMany(u => u.ViewedResidences)
                .HasForeignKey(vr => vr.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Keep cascading delete here

            builder.Entity<ViewedResidences>()
                .HasOne(vr => vr.Residence)
                .WithMany(r => r.ViewedResidences)
                .HasForeignKey(vr => vr.ResidenceId)
                .OnDelete(DeleteBehavior.Cascade); // Remove cascading delete here
        }
    }

}
