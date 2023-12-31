﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebApp.DataAccess.Repository
{
    public class LandlordReviewsRepository : Repository<LandlordReviews>, ILandlordReviewsRepository
    {
        private ApplicationDbContext _db;

        public LandlordReviewsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
