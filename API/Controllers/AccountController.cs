using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await UserExists(registerDto.UserName)) return BadRequest("User Already Taken");

            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key

            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //get user from db

            try
            {
                var usr = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.username);

                if (usr == null) return Unauthorized("Invalid UserName");

                using var hmac = new HMACSHA512(usr.PasswordSalt);

                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != usr.PasswordHash[i]) return Unauthorized("Invalid Password");
                }
                return new UserDto
                {
                    Username = usr.UserName,
                    Token = _tokenService.CreateToken(usr),
                    PhotoUrl = usr.Photos.FirstOrDefault(x => x.IsMain)?.Url
                };
            }

            catch (System.Exception)
            {

                throw;
            }

        }


        private async Task<bool> UserExists(String username)
        {

            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }


    }
}