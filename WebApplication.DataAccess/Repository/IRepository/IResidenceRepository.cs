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
        Residence Update(UpdateResidenceDTO obj);

        IQueryable<Residence> GetAllSearch(ResidenceSearch residenceSearch);
        IQueryable<Residence> UserResidences(string UserId);

        IQueryable<Residence> GetResidences();

        //PagedList<ResidenceDTO> ResidenceToResidenceDTO (PagedList<Residence> residenceList);
    }
}
