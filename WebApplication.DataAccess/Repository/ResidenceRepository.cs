﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebApp.DataAccess.Repository
{
    public class ResidenceRepository : Repository<Residence>, IResidenceRepository
    {
        private ApplicationDbContext _db;

        public ResidenceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Residence obj)
        {
            var objFromDb = _db.Residences.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.NumOfBeds = obj.NumOfBeds;
                objFromDb.City  = obj.City; 
                objFromDb.Country = obj.Country;    
                objFromDb.Neighborhood = obj.Neighborhood;
                objFromDb.Description = obj.Description;
                objFromDb.NumOfBathrooms = obj.NumOfBathrooms;
                objFromDb.ResidenceType = obj.ResidenceType;
                objFromDb.NumOfBedrooms = obj.NumOfBedrooms;
                objFromDb.LivingRoom = obj.LivingRoom;
                objFromDb.SquareMeters = obj.SquareMeters;
                objFromDb.Smoking = obj.Smoking;
                objFromDb.Pets = obj.Pets;
                objFromDb.Events = obj.Events;
                objFromDb.MinDaysForReservation = obj.MinDaysForReservation;
                if (obj.ImageURL != null)
                {
                    objFromDb.ImageURL = obj.ImageURL;
                }
            }
        }

        public IQueryable<Residence>  GetAllSearch(ResidenceSearch residenceSearch)
        {
            IQueryable<Residence> residences = _db.Residences.AsQueryable();
            if (!string.IsNullOrEmpty(residenceSearch.neighborhood))
                residences = residences.Where(x => x.Neighborhood == residenceSearch.neighborhood);
            if (!string.IsNullOrEmpty(residenceSearch.city))
                residences = residences.Where(x => x.City == residenceSearch.city);
            if (!string.IsNullOrEmpty (residenceSearch.country))
                residences = residences.Where(x => x.Country == residenceSearch.country);
            if (residenceSearch.numOfPeople.HasValue)
                residences = residences.Where(x => x.ResidentCapacity >= residenceSearch.numOfPeople);
            if (residenceSearch.Internet.HasValue)
                residences = residences.Where(x => x.Internet == residenceSearch.Internet);
            if (residenceSearch.Tv.HasValue)
                residences = residences.Where(x => x.Tv == residenceSearch.Tv);
            if (residenceSearch.Aircondition.HasValue)
                residences = residences.Where(x => x.Aircondition == residenceSearch.Aircondition);
            if (residenceSearch.Kitchen.HasValue)
                residences = residences.Where(x => x.Kitchen == residenceSearch.Kitchen);
            if (residenceSearch.ParkingSpot.HasValue)
                residences = residences.Where(x => x.ParkingSpot == residenceSearch.ParkingSpot);

            if (residenceSearch.From.HasValue)
            {
                IQueryable<Reservation> NotAvailableReservations = _db.Reservations.AsQueryable();
                NotAvailableReservations = NotAvailableReservations.Where(x => 
                           residenceSearch.From >= x.From && residenceSearch.From <= x.To 
                        || residenceSearch.To >= x.From && residenceSearch.To <= x.To 
                        || residenceSearch.From <= x.From && residenceSearch.To >= x.To);
                IEnumerable<Reservation> nReservations = NotAvailableReservations.ToList();
                foreach (Reservation reservation in nReservations)
                {
                    residences = residences.Where(x => x.Id != reservation.ResidenceId);
                }
            }
            return residences;
            //return await GetPagination(residences, residenceSearch.PageSize , residenceSearch.pageNumber);
        }

        public IQueryable<Residence> UserResidences(string UserID)
        {
            IQueryable<Residence> residences = _db.Residences.AsQueryable();
            residences = residences.Where(x => x.UserId == UserID);
            return residences;
        }
    }
}
