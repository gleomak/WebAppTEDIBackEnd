using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.DataAccess.Repository.IRepository
{
    public interface IResidenceRepository : IRepository<Residence>
    {
        void Update(Residence obj);

        IEnumerable<Residence> GetAllSearch(ResidenceSearch residenceSearch);
    }
}
