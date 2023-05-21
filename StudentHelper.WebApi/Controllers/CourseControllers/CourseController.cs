using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.CourseRequests;
using System.Security.Claims;


namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<StudentCourse> _studentCourseRepository;
        private readonly CourseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(IRepository<Course> courseRepository, IRepository<Seller> sellerRepository
            , IRepository<Student> studentRepository, IRepository<StudentCourse> studentCourseRepository
            , CourseContext context, UserManager<ApplicationUser> userManager)
        {
            _courseRepository = courseRepository;
            _sellerRepository = sellerRepository;
            _studentRepository = studentRepository;
            _studentCourseRepository = studentCourseRepository;
            _context = context;
            _userManager = userManager;
        }
        [HttpPost("create-course")]
        public async Task<Response> CreateCourse(CreateCourseRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userId, out var id);
            Seller seller = await _sellerRepository.GetByUserId(id);
            if (seller == null)
            {
                return new Response(400, false, "Ты не являешься продавцом!");
            }
            var course = new Course
            {
                Title = request.Title,
                Description = request.Description,
                IsFree = request.IsFree,
                Price = request.Price,
                IsPublished = false,
                Seller = seller,
                SellerId = seller.Id,
            };
            await _courseRepository.AddAsync(course);
            return new Response(200, true, "Вы успешно создали курс!");
        }
        [HttpGet("course/{id}")]
        public async Task<Response> GetCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return new Response(404, false, "Курс не найден!");
            }
            return new Response(200, true, $"{course.Title}");
        }
        [HttpDelete("delete-course/{id}")]
        public async Task<Response> DeleteCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return new Response(404, false, "Курс с таким айди не найден!");
            }
            await _courseRepository.DeleteAsync(id);
            return new Response(200, true, "Вы успешно удалили данный курс!");
        }
        [HttpPost("add-course-to-student")]
        public async Task<Response> AddCourseToStudent(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userId, out var id);

            var student = await _studentRepository.GetByUserId(id);
            var course = await _courseRepository.GetByIdAsync(courseId);

            if (student == null || course == null)
            {
                return new Response(404, false, "Одна из сущностей не найдена!");
            }

            var studentCourse = new StudentCourse
            {
                Student = student,
                Course = course
            };

            await _studentCourseRepository.AddAsync(studentCourse);

            return new Response(200, false, "Вы успешно связали сущности!");
        }
        [HttpPost("remove-course-from-student")]
        public async Task<Response> RemoveCourseFromStudent(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userId, out var id);

            var student = await _studentRepository.GetByUserId(id);
            var studentCourse = await _studentCourseRepository.FindManyToMany(student.Id, courseId);
            if (studentCourse == null)
            {
                return new Response(404, false, "У студента не найден данный курс!");
            }
            await _studentCourseRepository.RemoveAsync(studentCourse);
            return new Response(200, true, "Вы успешно удалили данный курс у студента!");

        }
        [HttpGet("courses/{courseId}/students")]
        public async Task<List<ApplicationUserDTO>> GetAllCourseStudents(int courseId)
        {
            var sortedStudentIds = _context.StudentCourses
                .Where(sc => sc.CourseId == courseId)
                .OrderBy(sc => sc.CourseId)
                .Select(sc => sc.StudentId)
                .ToList();
            var normalizedUsers = new List<ApplicationUserDTO>();
            for (int i = 0; i < sortedStudentIds.Count; i++)
            {
                var student = await _studentRepository.GetByIdAsync(sortedStudentIds[i]);
                var stringId = student.UserId.ToString();
                var user = await _userManager.FindByIdAsync(stringId);
                var normalizedUser = new ApplicationUserDTO
                {
                    UserName = user.UserName,
                    Id = user.Id,
                    StudentId = student.Id,
                    Email = user.Email
                };
                normalizedUsers.Add(normalizedUser);

            }
            return normalizedUsers;
        }

        [HttpGet("students/{studentId}/courses")]
        public async Task<List<Course>> GetAllStudentCourses(int studentId)
        {
            var sortedCourseIds = _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .OrderBy(sc => sc.StudentId)
                .Select(sc => sc.CourseId)
                .ToList();
            var courses = new List<Course>();
            for (int i = 0; i < sortedCourseIds.Count; i++)
            {
                var course = await _courseRepository.GetByIdAsync(sortedCourseIds[i]);
                courses.Add(course);
            }
            return courses;
        }
        [HttpGet("courses")]
        public async Task<List<Course>> GetAllCourses()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.ToList();
        }
    }
}
