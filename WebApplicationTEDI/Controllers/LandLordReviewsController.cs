using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebAppTEDI.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class LandLordReviewsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LandLordReviewsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("getDataJSON")]
        [Authorize(Roles ="Admin")]
        public IActionResult GetDataJSON() 
        {
            List<LandlordReviews> landlordReviews = _unitOfWork.LandlordReviews.GetAll().ToList();
            if (landlordReviews.Count == 0)
            {
                return NoContent();
            }
            string jsonData = JsonSerializer.Serialize(landlordReviews);

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
        [Authorize(Roles ="Admin")]
        public IActionResult GetDataXML()
        {
            List<LandlordReviews> landlordReviews = _unitOfWork.LandlordReviews.GetAll().ToList();
            if (landlordReviews.Count == 0)
            {
                return NoContent();
            }
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<LandlordReviews>));
                serializer.Serialize(stream, landlordReviews);

                stream.Seek(0, SeekOrigin.Begin);

                var result = new FileContentResult(stream.ToArray(), "application/xml")
                {
                    FileDownloadName = "landlordReviews.xml"
                };

                return result;
            }
        }
    }
}
