using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Requests.AdminRequests;

namespace StudentHelper.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("GetUserByUserName")]
        public async Task<UserResponse> GetUserByUserName(GetUserByNameRequest request)
        {
            User user;
            IEnumerable<string> roles;
            try
            {
                user = await _userManager.FindByNameAsync(request.UserName);
                roles = await _userManager.GetRolesAsync(user);
            }
            catch (Exception)
            {
                return new UserResponse(400, false, "User with this username, is not finded!", null, null, 0, null);
                throw;
            } 
            
            return new UserResponse(200, true, "User has been finded", user.UserName, user.Email, user.Id, roles.ToList());
        }
    }
}
