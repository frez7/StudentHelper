using Microsoft.AspNetCore.Http;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Requests.ProfileRequests;
using System.Security.Claims;

namespace StudentHelper.BL.Services
{
    public class ProfileService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileService(IRepository<Student> studentRepository, IHttpContextAccessor httpContextAccessor)
        {
            _studentRepository = studentRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> UpdateStudentProfile(UpdateProfileRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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

