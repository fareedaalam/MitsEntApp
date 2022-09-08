using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;

            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsers()
        {
            var user = await _userRepository.GetMembersAsync();           
            return Ok(user);

        }

        [Authorize]
        [HttpGet("{username}")]
        public async Task<ActionResult<AppUserDto>> GetUser(String username)
        {
            try
            {
               return  await _userRepository.GetMemberAsync(username);
               
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}