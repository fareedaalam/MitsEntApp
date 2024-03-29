

namespace API.Controllers
{

    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
             .Include(r => r.UserRoles)
             .ThenInclude(r => r.Role)
             .OrderBy(u => u.UserName)
             .Select(u => new
             {
                 u.Id,
                 username = u.UserName,
                 isActive = u.IsActive,
                 Roles = u.UserRoles.Select(r => r.Role.Name).ToList()

             }).ToListAsync();
            return Ok(users);
        }
        

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound("Could not fond user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded) return BadRequest("Failed to add to roles");
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("admin or moderate can see this");
        }

        [HttpPut("deactivate/{username}")]
        public async Task<ActionResult> DeActivate(string username)
        {

            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found");

            user.IsActive = user.IsActive == true ? false : true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest("Failed to " + user.IsActive + "to roles");
                     
            return Ok(result);          

        }

    
    }
}