using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Requests.ProfileRequests;
using System.Security.Claims;

namespace StudentHelper.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IRepository<Student> _studentRepository;
        public ProfileController(IRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }
        [HttpPut("profile/update")]
        public async Task<Response> UpdateStudentProfile(UpdateProfileRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var intId = int.Parse(userId);
            var student = await _studentRepository.GetByUserId(intId);
            student.AboutMe = request.AboutMe;
            student.City = request.City;
            student.Country = request.Country;
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            await _studentRepository.UpdateAsync(student);
            return new Response(200, true, "Вы успешно обновили данные о своем профиле");
        }
        [HttpGet("students/{studentId}/profile")]
        public async Task<Student> GetProfile(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new Exception("Студент не найден");
            }
            return student;
        }
    }
}
