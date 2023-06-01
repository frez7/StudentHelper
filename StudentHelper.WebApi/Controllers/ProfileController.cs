using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Requests.ProfileRequests;

namespace StudentHelper.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileService _service;

        public ProfileController(ProfileService service)
        {
            _service = service;
        }

        [HttpPut("profile/update")]
        public async Task<Response> UpdateStudentProfile(UpdateProfileRequest request)
        {
            return await _service.UpdateStudentProfile(request);
        }

        [HttpGet("students/{studentId}/profile")]
        public async Task<Student> GetProfile(int studentId)
        {
            return await _service.GetProfile(studentId);
        }
    }
}
