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
    public class ResidenceReviewsRepository : Repository<ResidenceReviews>, IResidenceReviewsRepository
    {
        private ApplicationDbContext _db;

        public ResidenceReviewsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
