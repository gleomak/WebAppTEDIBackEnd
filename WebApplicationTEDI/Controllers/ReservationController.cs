using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public ReservationController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> usermanager )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = usermanager;
        }


        [Authorize(Roles="Member")]
        [HttpPost ("postReservation")]
        public async void ReservationSavePost([FromQuery]ReservationDTO reservationDTO)
        {
            Reservation reservation=_mapper.Map<Reservation>(reservationDTO);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            Console.WriteLine(user.Id);

            reservation.UserId = user.Id;

            _unitOfWork.Reservation.Add(reservation);
            _unitOfWork.Save();

        }
         

    }
}
