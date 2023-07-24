using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;

namespace WebApp.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Residence = new ResidenceRepository(_db);
        }
        public IResidenceRepository Residence { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
