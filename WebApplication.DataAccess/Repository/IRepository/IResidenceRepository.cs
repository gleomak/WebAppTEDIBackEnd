using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.DTOs;
using WebApp.Models.Helpers;

namespace WebApp.DataAccess.Repository.IRepository
{
    public interface IResidenceRepository : IRepository<Residence>
    {
        void Update(Residence obj);

        IQueryable<Residence> GetAllSearch(ResidenceSearch residenceSearch);
        IQueryable<Residence> UserResidences(string UserId);

        //PagedList<ResidenceDTO> ResidenceToResidenceDTO (PagedList<Residence> residenceList);
    }
}
