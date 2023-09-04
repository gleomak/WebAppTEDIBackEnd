using Microsoft.EntityFrameworkCore;
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
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        private ApplicationDbContext _db;

        public ReservationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Reservation> ReservationsWithId(string id)
        {
            int resID = Int32.Parse(id);

            IQueryable<Reservation> reservations = _db.Reservations.AsQueryable();

            reservations = reservations.Where(x => x.ResidenceId == resID);

            return reservations.ToList();
        }

    }
}
