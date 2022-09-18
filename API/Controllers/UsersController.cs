using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {

        
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper,
            IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
           
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            //get the filter data
            var gender = await _unitOfWork.UserRepository.GetUserGender(User.GetUsername());
           // var knownas = await _unitOfWork.UserRepository.GetUserKnownAs(User.GetUsername());

            userParams.CurrentUsername = User.GetUsername();
            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = gender == "male" ? "female" : "male";

            if(string.IsNullOrEmpty(userParams.KnownAs))
                userParams.KnownAs = userParams.KnownAs == "member" ? "contestant" : "member";

            var user = await _unitOfWork.UserRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(user.CurrentPage, user.PageSize, user.TotalCount, user.TotalPages);
            return Ok(user);

        }


        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<AppUserDto>> GetUser(String username)
        {
            try
            {
                return await _unitOfWork.UserRepository.GetMemberAsync(username);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // [HttpGet("{username}", Name = "GetUser")]
        // public async Task<ActionResult<AppUserDto>> GetUser(string username)
        // {
        //     return await _unitOfWork.UserRepository.GetMemberAsync(username);
        // }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            //get the username form the token
            //var username=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.GetUsername();

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            //Map dto with usertable
            _mapper.Map(memberUpdateDto, user);
            _unitOfWork.UserRepository.Update(user);
            if (await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to update user");


        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            //Get the username form token
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _unitOfWork.Complete())
            {
                //return _mapper.Map<PhotoDto>(photo);
                //this will give the 201 status with currnt Api 
                return CreatedAtRoute("GetUser",
                new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");
        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("this is already your main photo");

            var curretnMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (curretnMain != null) curretnMain.IsMain = false;

            photo.IsMain = true;

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to set main photo");

        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }

    }
}