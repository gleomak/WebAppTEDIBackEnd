using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
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

        [HttpGet ("getReservationsForResidence")]
        public ActionResult<List<Reservation>> ResidenceRerervationGet([FromQuery]string ResidenceId)
        {
            IEnumerable<Reservation> reservations = _unitOfWork.Reservation.ReservationsWithId(ResidenceId);

            return reservations.ToList();
        }

        [HttpGet("getDataJSON")]
        [Authorize(Roles="Admin")]
        public IActionResult GetDataJSON()
        {
            List<Reservation> reservations = _unitOfWork.Reservation.GetAll().ToList();
            if (reservations.Count == 0)
            {
                return NoContent();
            }
            string jsonData = JsonSerializer.Serialize(reservations);

            // Set response headers to indicate a downloadable JSON file
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "data.json"
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            // Return JSON data as a FileResult with content type application/json
            return File(Encoding.UTF8.GetBytes(jsonData), "application/json");
        }

        [HttpGet("getDataXML")]
        [Authorize(Roles="Admin")]
        public IActionResult GetDataXML()
        {
            List<Reservation> reservations = _unitOfWork.Reservation.GetAll().ToList();
            if (reservations.Count == 0)
            {
                return NoContent();
            }
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Reservation>));
                serializer.Serialize(stream, reservations);

                stream.Seek(0, SeekOrigin.Begin);

                var result = new FileContentResult(stream.ToArray(), "application/xml")
                {
                    FileDownloadName = "Reservations.xml"
                };

                return result;
            }
        }


    }
}
