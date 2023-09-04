using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataAccess.Data;
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
        private readonly ApplicationDbContext   _context;
        public ReservationController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> usermanager, ApplicationDbContext context )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = usermanager;
            _context = context;
        }


        //[Authorize(Roles="Member")]
        [HttpPost ("postReservation")]
        public void ReservationSavePost([FromForm]ReservationDTO reservationDTO)
        {
            Reservation reservation=_mapper.Map<Reservation>(reservationDTO);
            
            _unitOfWork.Reservation.Add(reservation);
            _unitOfWork.Save();

        }
        
    }
}
