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
        public async Task<ActionResult<List<Residence>>> GetResidence([FromQuery] ResidenceSearch residenceSearch) {
            //Residence residence = new Residence();
            //residence.NumOfBeds = 1;
            //residence.City = "Athens";
            //residence.Country = "Greece";
            //residence.ResidentCapacity = 5;
            //residence.Neighborhood = "Nea Smyrni";
            //residence.NumOfBathrooms = 1;
            //residence.NumOfBedrooms = 2;
            //residence.ResidenceType = "Treehouse";
            //residence.LivingRoom = false;
            //residence.SquareMeters = 100;
            //residence.Description = "Description";
            //residence.Smoking = false;
            //residence.Pets = true;
            //residence.Events = true;
            //residence.Internet = false;
            //residence.Aircondition = true;
            //residence.Kitchen = true;
            //residence.ParkingSpot = true;
            //residence.Tv = false;
            //residence.MinDaysForReservation = 3;
            //residence.ImageURL = "KAPPP";
            //_unitOfWork.Residence.Add(residence);
            //_unitOfWork.Save();

            //Residence residence2 = new Residence();
            //residence2.NumOfBeds = 1;
            //residence2.City = "Athens";
            //residence2.Country = "Greece";
            //residence2.ResidentCapacity = 8;
            //residence2.Neighborhood = "Penteli";
            //residence2.NumOfBathrooms = 1;
            //residence2.NumOfBedrooms = 2;
            //residence2.ResidenceType = "Treehouse";
            //residence2.LivingRoom = false;
            //residence2.SquareMeters = 100;
            //residence2.Description = "Description";
            //residence2.Smoking = false;
            //residence2.Pets = true;
            //residence2.Events = true;
            //residence2.Internet = false;
            //residence2.Aircondition = true;
            //residence2.Kitchen = true;
            //residence2.ParkingSpot = true;
            //residence2.Tv = false;
            //residence2.MinDaysForReservation = 4;
            //residence2.ImageURL = "KAPPP";
            //_unitOfWork.Residence.Add(residence2);
            //_unitOfWork.Save();

            //Residence residence3 = new Residence();
            //residence3.NumOfBeds = 1;
            //residence3.City = "Patra";
            //residence3.Country = "Greece";
            //residence3.ResidentCapacity = 3;
            //residence3.Neighborhood = "Agia";
            //residence3.NumOfBathrooms = 1;
            //residence3.NumOfBedrooms = 2;
            //residence3.ResidenceType = "Treehouse";
            //residence3.LivingRoom = false;
            //residence3.SquareMeters = 100;
            //residence3.Description = "Description";
            //residence3.Smoking = false;
            //residence3.Pets = true;
            //residence3.Events = true;
            //residence3.Internet = false;
            //residence3.Aircondition = true;
            //residence3.Kitchen = true;
            //residence3.ParkingSpot = true;
            //residence3.Tv = false;
            //residence3.MinDaysForReservation = 1;
            //residence3.ImageURL = "KAPPP";
            //_unitOfWork.Residence.Add(residence3);
            //_unitOfWork.Save();

            //Residence residence4 = new Residence();
            //residence4.NumOfBeds = 1;
            //residence4.City = "Ioannina";
            //residence4.Country = "Greece";
            //residence4.ResidentCapacity = 3;
            //residence4.Neighborhood = "Agia";
            //residence4.NumOfBathrooms = 1;
            //residence4.NumOfBedrooms = 2;
            //residence4.ResidenceType = "Treehouse";
            //residence4.LivingRoom = false;
            //residence4.SquareMeters = 100;
            //residence4.Description = "Description";
            //residence4.Smoking = false;
            //residence4.Pets = true;
            //residence4.Events = true;
            //residence4.Internet = false;
            //residence4.Aircondition = true;
            //residence4.Kitchen = true;
            //residence4.ParkingSpot = true;
            //residence4.Tv = false;
            //residence4.MinDaysForReservation = 1;
            //residence4.ImageURL = "KAPPP";
            //_unitOfWork.Residence.Add(residence4);
            //_unitOfWork.Save();

            return _unitOfWork.Residence.GetAllSearch(residenceSearch).ToList();
            //return _unitOfWork.Residence.GetAll().ToList();
        }
        [HttpGet("{id}")]
        public  ActionResult<Residence> FindResidence(int id)
        {
            return _unitOfWork.Residence.GetFirstOrDefault(x => x.Id == id);
        }

        [HttpGet("Search")]
        public ActionResult<List<Residence>> GetResidenceSearch([FromQuery]ResidenceSearch residenceSearch)
        {
            return _unitOfWork.Residence.GetAllSearch(residenceSearch).ToList();
        }
    }
}
