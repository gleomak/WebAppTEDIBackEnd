using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Models.DTOs;
using WebApp.Models.Helpers;
using WebApp.Services;

namespace WebAppTEDI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatrixFactorizationController : ControllerBase
    {
        private readonly MatrixFactorization _matrixFactorization;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public MatrixFactorizationController(MatrixFactorization matrixFactorization, IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _matrixFactorization = matrixFactorization;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("getRecommendedResidences")]
        [Authorize]
        public async Task<ActionResult<PagedList<ResidenceDTO>>> GetRecommendedResidences([FromQuery] PaginationParams paginationParams)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var residences = _matrixFactorization.ResidenceRecomendations(user.Id);
            IQueryable<Residence> residencesQ = _unitOfWork.Residence.GetResidences();
            residencesQ = residencesQ.Where(r => residences.Contains(r));

            var residencesM = await PagedList<Residence>.ToPagedList(residencesQ, paginationParams.pageNumber, paginationParams.PageSize);
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
                var pictures = _unitOfWork.Image.GetAll(x => x.ResidenceId == res.Id).ToList();
                if (pictures != null)
                {
                    foreach (var p in pictures)
                    {
                        residenceDTO.ImageURL.Add(p.URL);
                    }
                }

                var reviews = _unitOfWork.ResidenceReviews.GetAll(x => x.ResidenceId == res.Id).ToList();
                foreach (var r in reviews)
                {
                    ResidenceReviewDTO resReview = new ResidenceReviewDTO
                    {
                        ResidenceId = r.ResidenceId,
                        Description = r.Description,
                        StarRating = r.StarRating.ToString(),
                        Username = r.Username,
                    };
                    residenceDTO.Reviewss.Add(resReview);
                }
                list.Add(residenceDTO);
            }
            var residencesDTOS = new PagedList<ResidenceDTO>(list, residencesM.Metadata.TotalCount, residencesM.Metadata.CurrentPage, residencesM.Metadata.PageSize);
            Response.AddPaginationHeader(residencesDTOS.Metadata);
            return residencesDTOS;
        }
    }
}
