using Microsoft.AspNetCore.Mvc;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebAppTEDI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReservationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        public void ReservationSavePost([FromQuery]ReservationDTO reservationDTO)
        {
            Reservation reservation = new Reservation();
            reservation.ResidenceId = reservationDTO.ResidenceId;
            reservation.From = reservationDTO.From;
            reservation.To = reservationDTO.To;

            _unitOfWork.Reservation.Add(reservation);
            _unitOfWork.Save();

        }


    }
}
