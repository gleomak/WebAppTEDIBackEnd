using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Models.DTOs;
using WebApp.Models.Helpers;
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
        private readonly IUnitOfWork _unitOfWork;    

        public AccountController(UserManager<User> userManager, TokenService tokenService, IMapper mapper, ImageService imageService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            User user = await _userManager.FindByNameAsync(loginDTO.Username);
            if(user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password)){
                return Unauthorized();
            }
            return new UserDTO {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                StreetAddress = user.StreetAddress,
                PhoneNumber = user.PhoneNumber,
                PictureURL = user.PictureURL,
                Token = await _tokenService.GenerateToken(user),
            };
        }

        [HttpPost("register")]
        //[ValidateAntiForgeryToken]
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
            if (registerDTO.Role.Equals("Host"))
            {
                await _userManager.AddToRoleAsync(user, "Host");
            } else if (registerDTO.Role.Equals("Member"))
            {
                await _userManager.AddToRoleAsync(user, "Member");

            } else
            {
                await _userManager.AddToRolesAsync(user, new[] { "Member", "Host" });
            }
            return StatusCode(201);
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                StreetAddress = user.StreetAddress,
                PhoneNumber = user.PhoneNumber,
                PictureURL = user.PictureURL,
                Token = await _tokenService.GenerateToken(user),
            };
        }

        [HttpGet("getUserDetails")]
        public async Task<ActionResult<User>> GetUserDetails(string username)
        {
            User user = await _userManager.FindByNameAsync(username);
            return user;
        }


        [Authorize]
        [HttpPost("editUserProfile")]
        public async Task<ActionResult> EditUserProfile([FromForm]UpdateUserDTO updateUser)
        {
            var user = await _userManager.FindByNameAsync(updateUser.Username);
            if(user == null)
            {
                return NotFound();
            }

            //foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(updateUser))
            //{
            //    string name = descriptor.Name;
            //    object value = descriptor.GetValue(updateUser);
            //    Console.WriteLine("{0}={1}", name, value);
            //}

            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.StreetAddress = updateUser.StreetAddress;
            user.PhoneNumber = updateUser.PhoneNumber;
            if (!string.IsNullOrEmpty(updateUser.NewPassword))
            {
                var result = await _userManager.ChangePasswordAsync(user, updateUser.Password, updateUser.NewPassword);
                if(!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return ValidationProblem();
                }
            }

            if (updateUser.File != null)
            {
                var imageUploadResult = await _imageService.AddImageAsync(updateUser.File);

                if (imageUploadResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageUploadResult.Error.Message });

                if (!string.IsNullOrEmpty(user.PublicId))
                    await _imageService.DeleteImageAsync(user.PublicId);

                user.PictureURL = imageUploadResult.SecureUrl.ToString();
                user.PublicId = imageUploadResult.PublicId;
            }
            _unitOfWork.Save();
            return StatusCode(201);
        }

        [HttpGet("retrieveUserList")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedList<UserDTO>>> GetUsers([FromQuery] PaginationParams paginationParams)
        {
            var userIQ =  _userManager.Users;
            var userListPaged = await PagedList<User>.ToPagedList(userIQ, paginationParams.pageNumber, paginationParams.PageSize);
            List<UserDTO> userDTOList = new List<UserDTO>();
            foreach (var user in userListPaged)
            {
                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userDTO.Roles = new List<string>(roles);
                //foreach(var role in roles)
                //{
                //    Console.WriteLine(role);
                //    userDTO.Roles.Append(role);
                //}
                userDTOList.Add(userDTO);
            }
            var userDTOListPaged = new PagedList<UserDTO>(userDTOList, userListPaged.Metadata.TotalCount, userListPaged.Metadata.CurrentPage, userListPaged.Metadata.PageSize);
            return userDTOListPaged;
        }
    }

    

}
