using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppUserDto> GetMemberAsync(string username)
        {
            return await _context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<AppUserDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<PagedList<AppUserDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();
            // .ProjectTo<AppUserDto>(_mapper.ConfigurationProvider)
            // .AsNoTracking()
            // .AsQueryable();
            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            //if(!String.IsNullOrEmpty(userParams.Gender))
                query = query.Where(u => u.Gender == userParams.Gender);
            //if(!String.IsNullOrEmpty(userParams.KnownAs))
                query = query.Where(u => u.KnownAs == userParams.KnownAs);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            //order by
            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<AppUserDto>.CreateAsync(query.ProjectTo<AppUserDto>(_mapper
                .ConfigurationProvider).AsNoTracking(),
                    userParams.PageNumber, userParams.PageSize);
        }

        public async Task<IEnumerable<AppUser>> GetUserAsync()
        {
            return await _context.Users
            .Include(x => x.Photos)
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);

        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<string> GetUserGender(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .Select(x => x.Gender).FirstOrDefaultAsync();
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public void DeActivateUser(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;

        }

        public async Task<IEnumerable<AppUserDto>> GetUserByKnownAs(UserParams userParam)
        {
            return await _context.Users
            .Include(x => x.Photos)
            .Where(x => x.KnownAs == userParam.KnownAs)
            .Where(x => x.IsActive == userParam.IsActive)
            .Where(x=>x.UserName.ToLower()!="admin")
            .Take(userParam.RegistrationCount)
            .ProjectTo<AppUserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();            
        }

        public async Task<string> GetUserByMobile(string mobile)
        {
            return await _context.Users                
                .Where(x => x.PhoneNumber == mobile)
                .Select(x => x.PhoneNumber).FirstOrDefaultAsync();
        }
    }
}