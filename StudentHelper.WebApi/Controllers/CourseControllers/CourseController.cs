using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
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

        public CourseController(IRepository<Course> courseRepository, IRepository<Seller> sellerRepository, IRepository<Student> studentRepository, IRepository<StudentCourse> studentCourseRepository)
        {
            _courseRepository = courseRepository;
            _sellerRepository = sellerRepository;
            _studentRepository = studentRepository;
            _studentCourseRepository = studentCourseRepository;
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
        //[HttpPost("get-all-student-courses")]
        //public async Task<List<StudentCourse>> GetAllStudentCourses(int studentId)
        //{
        //    var student = await _studentRepository.GetByIdAsync(studentId);
        //    var courses = ;
        //    return courses;
        //}

    }
}
