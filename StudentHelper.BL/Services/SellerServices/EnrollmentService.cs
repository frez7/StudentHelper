using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common.SellerResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using System.Security.Claims;

namespace StudentHelper.BL.Services.SellerServices
{
    /// <summary>
    /// Сервис для оплаты и отчетов
    /// </summary>
    public class EnrollmentService
    {
        private readonly IRepository<Enrollment> _enrollmentRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CourseContext _dbContext;

        public EnrollmentService(IRepository<Enrollment> enrollmentRepository, IRepository<Seller> sellerRepository,
            IRepository<Course> courseRepository, IHttpContextAccessor httpContextAccessor,
            IRepository<Student> studentRepository, CourseContext dbContext)
        {
            _enrollmentRepository = enrollmentRepository;
            _sellerRepository = sellerRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        
        public async Task<EnrollmentResponse> AddEnrollment(int courseId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var course = await _courseRepository.GetByIdAsync(courseId);
            var student = await _studentRepository.GetByUserId(id);
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
        public async Task<EnrollmentsResponse> GetSellerEnrollments()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
           
            var seller = await _sellerRepository.GetByUserId(id);
            if (seller == null)
            {
                throw new Exception("Ты не являешься продавцом!");
            }
            if (seller.UserId != id)
            {
                throw new Exception("Вы не можете получить данную информацию!");
            }
            var enrollments = _dbContext.Enrollments.Where(ent => ent.SellerId == seller.Id).ToList();
            var enrollmentsDTO = new List<EnrollmentDTO>();

            for (int i = 0; i < enrollments.Count; i++)
            {
                var course = await _courseRepository.GetByIdAsync(enrollments[i].CourseId);
                var student = await _studentRepository.GetByIdAsync(enrollments[i].StudentId);
                var enrollmentDTO = new EnrollmentDTO()
                {
                    Id = enrollments[i].Id,
                    CourseName = course.Title,
                    StudentName = $"{student.FirstName} {student.LastName}",
                    CreatedAt = enrollments[i].CreatedAt,
                    ReceivedMoney = enrollments[i].ReceivedMoney,
                };
                enrollmentsDTO.Add(enrollmentDTO);
            }
            return new EnrollmentsResponse(200, true, "Успешный запрос!", enrollmentsDTO);
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
