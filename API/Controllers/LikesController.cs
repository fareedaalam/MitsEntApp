using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Extentions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Entities;
using API.DTOs;
using API.Helpers;

namespace API.Controllers
{
   // [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;

        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var SourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (SourceUser.UserName == username) return BadRequest("You cannot like Yourself");

            var UserLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (UserLike != null) return BadRequest("You already like this user");

            UserLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            SourceUser.LikedUsers.Add(UserLike);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Faild to like user");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUsersLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId= User.GetUserId();
            var users = await _likesRepository.GetUserLike(likesParams);
            Response.AddPaginationHeader(users.CurrentPage,
                users.PageSize,users.TotalCount,users.TotalPages);
            

            return Ok(users);
        }
    }
}