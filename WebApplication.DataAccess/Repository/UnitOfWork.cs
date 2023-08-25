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
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Residence = new ResidenceRepository(_db);
            Reservation = new ReservationRepository(_db);

            LandlordReviews = new LandlordReviewsRepository(_db);
            ResidenceReviews = new ResidenceReviewsRepository(_db);
        }
        public IResidenceRepository Residence { get; private set; }
        public IReservationRepository Reservation { get; private set; }

        public ILandlordReviewsRepository LandlordReviews { get; private set; }
        public IResidenceReviewsRepository ResidenceReviews { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
