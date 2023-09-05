using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Models.DTOs;
using WebApp.Models.Helpers;

namespace WebAppTEDI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidenceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ResidenceController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ResidenceDTO>>> GetResidence([FromQuery] ResidenceSearch residenceSearch) {

            var residences = _unitOfWork.Residence.GetAllSearch(residenceSearch);
            var residencesM = await PagedList<Residence>.ToPagedList(residences, residenceSearch.pageNumber, residenceSearch.PageSize);
            List<ResidenceDTO> list = new List<ResidenceDTO>();
            foreach (var res in residencesM)
            {
                ResidenceDTO residenceDTO = _mapper.Map<ResidenceDTO>(res);
                var reservations = _unitOfWork.Reservation.GetAll(x => x.ResidenceId == res.Id);
                foreach (var r in reservations)
                {
                    ReservationFromTo reservationFromTo = new ReservationFromTo();
                    reservationFromTo.From = r.From.ToString();
                    reservationFromTo.To = r.To.ToString();
                    residenceDTO.ReservationFromTo.Add(reservationFromTo);
                }
                list.Add(residenceDTO);
            }
            var residencesDTOS = new PagedList<ResidenceDTO>(list, residencesM.Metadata.TotalCount, residencesM.Metadata.CurrentPage, residencesM.Metadata.PageSize);
            Response.AddPaginationHeader(residencesDTOS.Metadata);
            return residencesDTOS;
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
