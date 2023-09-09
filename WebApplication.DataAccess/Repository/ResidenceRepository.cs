using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Models.DTOs;
using WebApp.Models.Helpers;

namespace WebApp.DataAccess.Repository
{
    public class ResidenceRepository : Repository<Residence>, IResidenceRepository
    {
        private ApplicationDbContext _db;

        public ResidenceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Residence Update(UpdateResidenceDTO obj)
        {
            var intID = int.Parse(obj.Id);
            Console.WriteLine(intID.ToString());
            var objFromDb = _db.Residences.FirstOrDefault(u => u.Id == intID);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.City = obj.City;
                objFromDb.Country = obj.Country;
                objFromDb.Neighborhood = obj.Neighborhood;
                objFromDb.ResidentCapacity = obj.ResidentCapacity;
                objFromDb.NumOfBeds = obj.NumOfBeds;
                objFromDb.Description = obj.Description;
                objFromDb.NumOfBathrooms = obj.NumOfBathrooms;
                objFromDb.ResidenceType = obj.ResidenceType;
                objFromDb.NumOfBedrooms = obj.NumOfBedrooms;
                objFromDb.LivingRoom = obj.LivingRoom;
                objFromDb.SquareMeters = obj.SquareMeters;
                objFromDb.Smoking = obj.Smoking;
                objFromDb.Pets = obj.Pets;
                objFromDb.Events = obj.Events;
                objFromDb.Internet = obj.Internet;
                objFromDb.Aircondition = obj.Aircondition;
                objFromDb.Kitchen = obj.Kitchen;
                objFromDb.ParkingSpot = obj.ParkingSpot;
                objFromDb.Tv = obj.Tv;
                objFromDb.CostPerNight  = obj.CostPerNight;
                objFromDb.Address = obj.Address;
                objFromDb.MinDaysForReservation = obj.MinDaysForReservation;
                return objFromDb;
            }
            Console.WriteLine("IS NULL");
            return null;
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

        //public PagedList<ResidenceDTO> ResidenceToResidenceDTO(PagedList<Residence> residencePagedList)
        //{
        //    List<ResidenceDTO> list = new List<ResidenceDTO>();
        //    foreach (var res in residencePagedList)
        //    {
        //        ResidenceDTO residenceDTO = _mapper.Map<ResidenceDTO>(res);
        //        var reservations = _db.Reservations.AsQueryable();
        //        IEnumerable<Reservation> residenceReservations =  reservations.Where(x => x.ResidenceId == res.Id).ToList();  

        //        foreach (var r in residenceReservations)
        //        {
        //            ReservationFromTo reservationFromTo = new ReservationFromTo();
        //            reservationFromTo.From = r.From.ToString();
        //            reservationFromTo.To = r.To.ToString();
        //            residenceDTO.ReservationFromTo.Add(reservationFromTo);
        //        }
        //        var pictures = _db.Images.AsQueryable();
        //        IEnumerable<Image> picturesOfResidence = pictures.Where(x => x.ResidenceId == res.Id).ToList();
        //        if (pictures != null)
        //        {
        //            foreach (var p in pictures)
        //            {
        //                residenceDTO.ImageURL.Add(p.URL);
        //            }
        //            list.Add(residenceDTO);
        //        }
        //    }
        //    var residencesDTOS = new PagedList<ResidenceDTO>(list, residencePagedList.Metadata.TotalCount, residencePagedList.Metadata.CurrentPage, residencePagedList.Metadata.PageSize);
        //    return residencesDTOS;
        //}
    }
}
