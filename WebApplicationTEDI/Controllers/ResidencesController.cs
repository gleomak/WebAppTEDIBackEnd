using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebAppTEDI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidencesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResidencesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<List<Residence>>> GetResidence() {
            //Residence residence = new Residence();
            //residence.NumOfBeds = 1;
            //residence.NumOfBathrooms = 1;
            //residence.NumOfBedrooms = 2;
            //residence.ResidenceType = "Treehouse";
            //residence.LivingRoom = false;
            //residence.SquareMeters = 100;
            //residence.Description = "Description";
            //residence.Smoking = false;
            //residence.Pets = true;
            //residence.Events = true;
            //residence.MinDaysForReservation = 1;
            //residence.ImageURL = "KAPPP";
            //_unitOfWork.Residence.Add(residence);
            //_unitOfWork.Save();

            //Residence residence2 = new Residence();
            //residence2.NumOfBeds = 1;
            //residence2.NumOfBathrooms = 1;
            //residence2.NumOfBedrooms = 2;
            //residence2.ResidenceType = "Treehouse";
            //residence2.LivingRoom = false;
            //residence2.SquareMeters = 100;
            //residence2.Description = "Description";
            //residence2.Smoking = false;
            //residence2  .Pets = true;
            //residence2.Events = true;
            //residence2.MinDaysForReservation = 1;
            //residence2.ImageURL = "KEEPPP";
            //_unitOfWork.Residence.Add(residence2);
            //_unitOfWork.Save();
            return _unitOfWork.Residence.GetAll().ToList();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Residence>> FindResidence(int id)
        {
            return _unitOfWork.Residence.GetFirstOrDefault(x => x.Id == id);
        }
    }
}
