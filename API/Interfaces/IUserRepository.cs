using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        // Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUserAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagedList<AppUserDto>> GetMembersAsync(UserParams userParams);
        Task<AppUserDto> GetMemberAsync(string username);
        Task<string> GetUserGender(string username);
        void DeActivateUser(AppUser user);
        Task<IEnumerable<AppUserDto>> GetUserByKnownAs(UserParams userParam);

        Task<string> GetUserByMobile(string mobile);

    }
}