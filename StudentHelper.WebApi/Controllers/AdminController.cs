using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Models.Common;
using StudentHelper.WebApi.Managers;

namespace StudentHelper.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminManager _adminManager;

        public AdminController(AdminManager adminManager)
        {
            _adminManager = adminManager;
        }

        [HttpPost("add-to-manager")]
        public async Task<Response> AddToManager(string username)
        {
            return await _adminManager.AddToManager(username);
        }

        [HttpPost("add-to-admin")]
        public async Task<Response> AddToAdmin(string username)
        {
            return await _adminManager.AddToAdmin(username);
        }

        [HttpGet("GetUserByUserName")]
        public async Task<UserResponse> GetUserByUserName(string username)
        {
            return await _adminManager.GetUserByUserName(username);
        }
    }
}

