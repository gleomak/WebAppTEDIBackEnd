using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Residence
    {
        public int Id { get; set; }
        [Required]
        public string? Neighborhood { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Country { get; set; }
        [Required]
        public int ResidentCapacity { get; set; }

        [Required]
        public int NumOfBeds { get; set; }
        [Required]
        public int NumOfBathrooms { get; set; }
        [Required]
        public string? ResidenceType { get; set; }
        [Required]
        public int NumOfBedrooms { get; set; }
        [Required]
        public bool LivingRoom { get; set; }
        [Required]
        public int SquareMeters { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public bool Smoking { get; set; }
        [Required]
        public bool Pets { get; set; }
        [Required]
        public bool Events { get; set; }
        [Required]
        public bool Internet { get; set; }
        [Required]
        public bool Aircondition { get; set; }
        [Required]
        public bool Kitchen { get; set; }
        [Required]
        public bool ParkingSpot { get; set; }
        [Required]
        public bool Tv { get; set; }

        [Required]
        public int MinDaysForReservation { get; set; }
        [Required]
        public int CostPerNight { get; set; }
        [ValidateNever]
        public string? ImageURL { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public string? Address { get; set; }
        
        public ICollection<Reservation> Reservations { get;} = new List<Reservation>(); 

    }
}
