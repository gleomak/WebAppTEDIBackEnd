using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Models.DTOs;

namespace WebAppTEDI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewedResidencesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public ViewedResidencesController(IUnitOfWork unitOfWork, UserManager<User> manager)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("postViewedResidence")]
        public ActionResult PostViewedResidence([FromForm] ViewedResidenceDTO viewedResidenceDTO)
        {
            ViewedResidences viewedResidences = new ViewedResidences
            {
                UserId = viewedResidenceDTO.UserId, 
                ResidenceId = viewedResidenceDTO.ResidenceId,   
            };
            _unitOfWork.ViewedResidences.Add(viewedResidences);
            _unitOfWork.Save();
            return Ok();
        }
    }
}
