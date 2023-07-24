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
    }
}
