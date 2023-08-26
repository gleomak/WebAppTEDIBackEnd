using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebAppTEDI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidenceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResidenceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Residence>>> GetResidence([FromQuery] ResidenceSearch residenceSearch) {
            
            var residences = _unitOfWork.Residence.GetAllSearch(residenceSearch);
            var residencesM = await PagedList<Residence>.ToPagedList(residences, residenceSearch.pageNumber, residenceSearch.PageSize);
            Response.AddPaginationHeader(residencesM.Metadata);
            return residencesM;
            //return _unitOfWork.Residence.GetAll().ToList();
        }
        [HttpGet("{id}")]
        public  ActionResult<Residence> FindResidence(int id)
        {
            return _unitOfWork.Residence.GetFirstOrDefault(x => x.Id == id);
        }

        //[HttpGet("Search")]
        //public ActionResult<List<Residence>> GetResidenceSearch([FromQuery]ResidenceSearch residenceSearch)
        //{
        //    return _unitOfWork.Residence.GetAllSearch(residenceSearch).ToList();
        //}
    }
}
