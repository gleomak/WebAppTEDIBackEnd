﻿using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password)) {
                return Unauthorized();
            }
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDTO {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                StreetAddress = user.StreetAddress,
                PhoneNumber = user.PhoneNumber,
                PictureURL = user.PictureURL,
                Roles = new List<string>(roles),
                RoleAuthorized = user.RoleAuthorized,
                Token = await _tokenService.GenerateToken(user),
            };
        }

        [HttpPost("register")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            var user = new User {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PhoneNumber = registerDTO.PhoneNumber,
                StreetAddress = registerDTO.StreetAddress,
                RoleAuthorized = registerDTO.Role.Equals("Member") ? true : false,
            };
            //var user = _mapper.Map<User>(registerDTO);
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            if (registerDTO.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(registerDTO.File);
                if (imageResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });
                user.PictureURL = imageResult?.SecureUrl.ToString();
                user.PublicId = imageResult?.PublicId;
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
            var roles = await _userManager.GetRolesAsync(user);
            Console.WriteLine(roles);
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
                Roles = new List<string>(roles),
                RoleAuthorized = user.RoleAuthorized,
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
        public async Task<ActionResult> EditUserProfile([FromForm] UpdateUserDTO updateUser)
        {
            var user = await _userManager.FindByNameAsync(updateUser.Username);
            if (user == null)
            {
                return NotFound();
            }
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.StreetAddress = updateUser.StreetAddress;
            user.PhoneNumber = updateUser.PhoneNumber;
            if (!string.IsNullOrEmpty(updateUser.NewPassword))
            {
                var result = await _userManager.ChangePasswordAsync(user, updateUser.Password, updateUser.NewPassword);
                if (!result.Succeeded)
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
            var userIQ = _userManager.Users;
            var userListPaged = await PagedList<User>.ToPagedList(userIQ, paginationParams.pageNumber, paginationParams.PageSize);
            List<UserDTO> userDTOList = new List<UserDTO>();
            foreach (var user in userListPaged)
            {
                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userDTO.Roles = new List<string>(roles);
                userDTOList.Add(userDTO);
            }
            var userDTOListPaged = new PagedList<UserDTO>(userDTOList, userListPaged.Metadata.TotalCount, userListPaged.Metadata.CurrentPage, userListPaged.Metadata.PageSize);
            Response.AddPaginationHeader(userDTOListPaged.Metadata);
            return userDTOListPaged;
        }

        [HttpPost("authorizeUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AuthorizeUser([FromForm] string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            user.RoleAuthorized = true;
            _unitOfWork.Save();
            return StatusCode(201);
        }

        [HttpGet("getHostResidences")]
        [Authorize(Roles = "Host")]
        public async Task<ActionResult<PagedList<ResidenceDTO>>> getHostResidences([FromQuery] PaginationParams pagination)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var residences = _unitOfWork.Residence.UserResidences(user.Id);
            var pagedResidences = await PagedList<Residence>.ToPagedList(residences, pagination.pageNumber, pagination.PageSize);
            List<ResidenceDTO> list = new List<ResidenceDTO>();

            foreach (var res in pagedResidences)
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
            var residencesDTOS = new PagedList<ResidenceDTO>(list, pagedResidences.Metadata.TotalCount, pagedResidences.Metadata.CurrentPage, pagedResidences.Metadata.PageSize);
            Response.AddPaginationHeader(residencesDTOS.Metadata);
            return residencesDTOS;
        }

        [HttpGet("getHost")]
        public async Task<ActionResult<HostDTO>> getHostProf([FromQuery] string ResidenceId)
        {
            var resIdInt = Convert.ToInt32(ResidenceId);
            var residence = _unitOfWork.Residence.GetFirstOrDefault(x => x.Id == resIdInt);
            var user = await _userManager.FindByIdAsync(residence.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var UserReviews = _unitOfWork.LandlordReviews.GetAll(x => x.UserId == user.Id);

            var meanRating = 0.0;

            if (UserReviews.Count() != 0)
            {
                double sum = 0;
                foreach (var item in UserReviews)
                {
                    sum += item.StarRating;
                }
                meanRating = sum / UserReviews.Count();
            }

            var hostDTO = new HostDTO();
            hostDTO.Rating = meanRating;
            hostDTO.Username = user.UserName;
            hostDTO.ImageURL = user.PictureURL;
            hostDTO.HostReviews = UserReviews.ToList();

            return hostDTO;

        }

        [HttpPost("postLandlordReview")]
        [Authorize(Roles = "Member")]
        public async Task<ActionResult> PostLandlordReview([FromForm] LandlordReviewDTO landlordReviewDTO)
        {
            User Host = await _userManager.FindByNameAsync(landlordReviewDTO.HostName);
            LandlordReviews lreview = new LandlordReviews
            {
                Description = landlordReviewDTO.Description,
                StarRating = double.Parse(landlordReviewDTO.StarRating),
                UserId = Host.Id,
                ReviewByUser = landlordReviewDTO.ReviewByUser,
            };

            _unitOfWork.LandlordReviews.Add(lreview);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpPost("postMessage")]
        [Authorize]
        public async Task<ActionResult> postMessage([FromForm] AddMessageDTO addMessageDTO) {
            User RecipientUser = await _userManager.FindByNameAsync(addMessageDTO.RecipientUsername);
            Message message = new Message
            {
                UserId = RecipientUser.Id,
                MessageBody = addMessageDTO.MessageBody,
                SenderUsername = User.Identity.Name,
                ResidenceTitle = addMessageDTO.ResidenceTitle,
            };
            _unitOfWork.Message.Add(message);
            _unitOfWork.Save();
            return StatusCode(201);
        }

        [HttpGet("getUserMessages")]
        [Authorize]
        public async Task<ActionResult<PagedList<MessageDTO>>> GetUserMessages([FromQuery] PaginationParams pagination, string? ResidenceTitle)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userMessages = _unitOfWork.Message.GetAllUserMessages(user.Id, ResidenceTitle);
            var userMessagesV2 = await PagedList<Message>.ToPagedList(userMessages, pagination.pageNumber, pagination.PageSize);
            List<MessageDTO> messages = new List<MessageDTO>();
            int lastIndex = userMessagesV2.Count - 1;
            for (int i = lastIndex; i >= 0; i--)
            {
                var item = userMessagesV2[i];
                User senderUsername = await _userManager.FindByNameAsync(item.SenderUsername);
                MessageDTO message = new MessageDTO
                {
                    Id = item.Id,
                    Message = item.MessageBody,
                    ResidenceTitle = item.ResidenceTitle,
                    SenderUsername = item.SenderUsername,
                    SenderImageURL = senderUsername.PictureURL
                };

                messages.Add(message);
            }
            var PagedUserMessages = new PagedList<MessageDTO>(messages, userMessagesV2.Metadata.TotalCount, userMessagesV2.Metadata.CurrentPage, userMessagesV2.Metadata.PageSize);
            Response.AddPaginationHeader(PagedUserMessages.Metadata);
            return PagedUserMessages;
        }

        [HttpDelete("deleteUserMessage/{Id}")]
        [Authorize]
        public IActionResult DeleteUserMessage(int Id)
        {
            Message message = _unitOfWork.Message.GetFirstOrDefault(x => x.Id == Id);
            if (message == null)
                return NotFound();
            _unitOfWork.Message.Remove(message);
            _unitOfWork.Save();
            return Ok();

        }

        [HttpPost("postUserSearchedNeighborhoods")]
        [Authorize]
        public async Task<IActionResult> PostUserSearchedNeighborhood([FromForm] string Neighborhood)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            SearchedNeighborhoods searchedNeighborhoods = new SearchedNeighborhoods
            {
                UserId = user.Id,
                Neighborhood = Neighborhood
            };
            _unitOfWork.SearchedNeighborhoods.Add(searchedNeighborhoods);
            _unitOfWork.Save();
            return Ok();
        }
    }

}
