using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Requests.AdminRequests;

namespace StudentHelper.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IRepository<Student> _studentRepository;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, IRepository<Student> studentRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _studentRepository = studentRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("GetUserByUserName")]
        public async Task<UserResponse> GetUserByUserName(GetUserByNameRequest request)
        {
            ApplicationUser user;
            IEnumerable<string> roles;
            Student student = new Student();
            int studentId = 0;
            try
            {
                user = await _userManager.FindByNameAsync(request.UserName);
                roles = await _userManager.GetRolesAsync(user);
                student = await _studentRepository.GetByUserId(user.Id);
                studentId = student.Id;
                
            }
            catch (Exception)
            {
                return new UserResponse(400, false, "User with this username, is not finded!", null, null, 0, null, 0);
                throw;
            } 
            
            return new UserResponse(200, true, "User has been finded", user.UserName, user.Email, user.Id, roles.ToList(), studentId);
        }
    }
}
