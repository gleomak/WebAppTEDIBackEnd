using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Models.DTOs;
using WebApp.Models.Helpers;
using WebApp.Services;

namespace WebAppTEDI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidenceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;
        public ResidenceController(IUnitOfWork unitOfWork, IMapper mapper, ImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
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
                var pictures = _unitOfWork.Image.GetAll(x => x.ResidenceId == res.Id).ToList();
                if (pictures != null)
                {
                    foreach (var p in pictures)
                    {
                        residenceDTO.ImageURL.Add(p.URL);
                    }
                    list.Add(residenceDTO);
                }
            }
            var residencesDTOS = new PagedList<ResidenceDTO>(list, residencesM.Metadata.TotalCount, residencesM.Metadata.CurrentPage, residencesM.Metadata.PageSize);
            Response.AddPaginationHeader(residencesDTOS.Metadata);
            return residencesDTOS;
            //return _unitOfWork.Residence.GetAll().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<ResidenceDTO> FindResidence(int id)
        {
            var residence = _unitOfWork.Residence.GetFirstOrDefault(x => x.Id == id);
            ResidenceDTO residenceDTO = _mapper.Map<ResidenceDTO>(residence);

            var reservations = _unitOfWork.Reservation.GetAll(x => x.ResidenceId == residence.Id);

            foreach (var r in reservations)
            {
                ReservationFromTo reservationFromTo = new ReservationFromTo();
                reservationFromTo.From = r.From.ToString();
                reservationFromTo.To = r.To.ToString();
                residenceDTO.ReservationFromTo.Add(reservationFromTo);
            }
            var pictures = _unitOfWork.Image.GetAll(x => x.ResidenceId == residence.Id).ToList();
            foreach (var p in pictures)
            {
                residenceDTO.ImageURL.Add(p.URL);
            }
            return residenceDTO;
        }

        [HttpPost("createResidence")]
        [Authorize(Roles = "Host")]
        public async Task<ActionResult> createResidence([FromForm]CreateResidenceDTO createResidenceDTO)
        {
            Residence residence = _mapper.Map<Residence>(createResidenceDTO);
            _unitOfWork.Residence.Add(residence);

            if (createResidenceDTO.Files != null)
            {
                foreach(var file in createResidenceDTO.Files)
                {
                    var imageResult = await _imageService.AddImageAsync(file);
                    if (imageResult.Error != null)
                        return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });
                    var image = new Image
                    {
                        URL = imageResult.SecureUrl.ToString(),
                        PublicId = imageResult.PublicId,
                    };
                    _unitOfWork.Image.Add(image);
                    residence.Images.Add(image);
                }
            }
            _unitOfWork.Save();
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Host")]
        public async Task<ActionResult> DeleteResidence(int id)
        {
            var residence = _unitOfWork.Residence.GetFirstOrDefault(x => x.Id == id);
            if (residence == null) return NotFound();
            var residencePictures = _unitOfWork.Image.GetAll(x => x.ResidenceId == id);
            foreach(var picture in residencePictures)
            {
                await _imageService.DeleteImageAsync(picture.PublicId);
                _unitOfWork.Image.Remove(picture);
            }
            _unitOfWork.Residence.Remove(residence);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpPost("updateResidence")]
        public async Task<ActionResult> UpdateResidence([FromForm] UpdateResidenceDTO updateResidenceDTO)
        {

            Residence residence = _unitOfWork.Residence.Update(updateResidenceDTO);
            if(residence == null)
            {
                return NotFound();
            }
            if(updateResidenceDTO.FilesToAdd != null)
            {
                foreach(var  file in updateResidenceDTO.FilesToAdd)
                {
                    var imageResult = await _imageService.AddImageAsync(file);
                    if (imageResult.Error != null)
                        return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });
                    var image = new Image
                    {
                        URL = imageResult.SecureUrl.ToString(),
                        PublicId = imageResult.PublicId,
                    };
                    _unitOfWork.Image.Add(image);
                    residence.Images.Add(image);
                }
            }
            if(updateResidenceDTO.ImagesToDelete != null)
            {
                foreach( var image in updateResidenceDTO.ImagesToDelete)
                {
                    Console.WriteLine(image);
                    Image imageToDelete = _unitOfWork.Image.GetFirstOrDefault(x => x.URL == image);
                    _unitOfWork.Image.Remove(imageToDelete);
                }
            }
            _unitOfWork.Save();
            return StatusCode(201);
        }

        [HttpGet("getDataXML")]
        public IActionResult GetDataXML()
        {
            List<Residence> residences = _unitOfWork.Residence.GetAll().ToList();
            if(residences.Count == 0)
            {
                return NoContent();
            }
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Residence>));
                serializer.Serialize(stream, residences);

                stream.Seek(0, SeekOrigin.Begin);

                var result = new FileContentResult(stream.ToArray(), "application/xml")
                {
                    FileDownloadName = "Residences.xml"
                };

                return result;
            }

        }

        [HttpGet("getDataJSON")]
        public IActionResult GetDataJSON()
        {
            List<Residence> residences = _unitOfWork.Residence.GetAll().ToList();
            if (residences.Count == 0)
            {
                return NoContent();
            }
            string jsonData = JsonSerializer.Serialize(residences);

            // Set response headers to indicate a downloadable JSON file
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "data.json"
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            // Return JSON data as a FileResult with content type application/json
            return File(Encoding.UTF8.GetBytes(jsonData), "application/json");
        }
    }
}
