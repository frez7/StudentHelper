using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using System.Security.Claims;

namespace StudentHelper.BL.Services.SellerServices
{
    public class EnrollmentService
    {
        private readonly IRepository<Enrollment> _enrollmentRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EnrollmentService(IRepository<Enrollment> enrollmentRepository, IRepository<Seller> sellerRepository,
            IRepository<Course> courseRepository, IHttpContextAccessor httpContextAccessor, IRepository<Student> studentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _sellerRepository = sellerRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<EnrollmentResponse> AddEnrollment(int courseId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            Course course = await _courseRepository.GetByIdAsync(courseId);
            Student student = await _studentRepository.GetByUserId(id);
            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = student.Id,
                SellerId = course.SellerId,
                ReceivedMoney = course.Price,
                CreatedAt = DateTime.UtcNow,
            };
            await _enrollmentRepository.AddAsync(enrollment);
            return new EnrollmentResponse(200, true, "Запись успешна!", enrollment.Id);
        }
        public async Task<EnrollmentDTO> GetEnrollment(int enrollmentId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var seller = await _sellerRepository.GetByUserId(id);
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                throw new Exception("Данного зачисления не существует!");
            }
            var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);
            var student = await _studentRepository.GetByIdAsync(enrollment.StudentId);
            if (seller == null || seller.Id != enrollment.SellerId)
            {
                throw new Exception("Вы не можете получить данную информацию!");
            }
            var enrollmentDTO = new EnrollmentDTO
            {
                CourseName = course.Title,
                StudentName = $"{student.FirstName} {student.LastName}",

            };
            return enrollmentDTO;
        }
    }
}
