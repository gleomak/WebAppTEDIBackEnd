using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Models.DTOs;
using WebApp.Services;

namespace WebAppTEDI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public AccountController(UserManager<User> userManager, TokenService tokenService, IMapper mapper, ImageService imageService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _imageService = imageService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.Username);
            if(user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password)){
                return Unauthorized();
            }
            return new UserDTO{
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm]RegisterDTO registerDTO)
        {
            var user = new User {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PhoneNumber = registerDTO.PhoneNumber,
                StreetAddress = registerDTO.StreetAddress,

            };
            //var user = _mapper.Map<User>(registerDTO);
            if (registerDTO.File != null)
            {
                var imageResult =  await _imageService.AddImageAsync(registerDTO.File);
                if(imageResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });
                user.PictureURL = imageResult?.SecureUrl.ToString();
                user.PublicId = imageResult?.PublicId;
            }

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            await _userManager.AddToRoleAsync(user, "Member");
            return StatusCode(201);
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return new UserDTO
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user)
            };
        }
    }
}
