
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.CourseRequests;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace StudentHelper.BL.Services.CourseServices
{
    public class CourseService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<StudentCourse> _studentCourseRepository;
        private readonly CourseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PageService _pageService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseService(IRepository<Course> courseRepository, IRepository<Seller> sellerRepository,
            IRepository<Student> studentRepository, IRepository<StudentCourse> studentCourseRepository,
            CourseContext context, UserManager<ApplicationUser> userManager, PageService pageService,
            IHttpContextAccessor httpContextAccessor)
        {
            _courseRepository = courseRepository;
            _sellerRepository = sellerRepository;
            _studentRepository = studentRepository;
            _studentCourseRepository = studentCourseRepository;
            _context = context;
            _userManager = userManager;
            _pageService = pageService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> CreateCourse([Microsoft.AspNetCore.Mvc.FromBody] CreateCourseRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
                Seller = seller,
                SellerId = seller.Id,
            };
            await _courseRepository.AddAsync(course);
            return new Response(200, true, "Вы успешно создали курс!");
        }
        public async Task<CourseDTO> GetCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                throw new Exception("Курс с таким айди не найден!");
            }
            var pages = await _pageService.GetAllPagesByCourseId(course.Id);
            var courseDTO = new CourseDTO
            {
                Description = course.Description,
                IsFree = course.IsFree,
                Price = course.Price,
                Title = course.Title,
                SellerId = course.SellerId,
                Pages = pages,
            };
            return courseDTO;
        }
        public async Task<Response> UpdateCourseImage(int id, IFormFile image)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return new Response(404, false, "Not Found!");
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var parsedId);
            var seller = await _sellerRepository.GetByUserId(parsedId);
            if (seller.Id != course.SellerId)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
            }
            course.Image = image;
            string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(rootPath, "images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + course.Image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await course.Image.CopyToAsync(stream);
            }

            course.ImageURL = "/images/" + uniqueFileName;
            await _courseRepository.UpdateAsync(course);
            return new Response(200, true, course.ImageURL);
        }
        public async Task<IActionResult> GetImage(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null || course.ImageURL == null)
            {
                throw new Exception("Изображение не найдено!");
            }
            string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string imagePath = Path.Combine(rootPath, course.ImageURL.TrimStart('/'));
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "image/jpeg");
        }
        public async Task<Response> UpdateCourse(int id, [System.Web.Http.FromBody] CreateCourseRequest request)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int parsedUserId = int.Parse(userId);
            var seller = await _sellerRepository.GetByIdAsync(course.SellerId);
            if (seller == null)
            {
                return new Response(400, false, "Вы не можете изменить данный курс!!!");
            }
            else if (seller.UserId != parsedUserId)
            {
                return new Response(400, false, "Вы не можете изменить данный курс!");
            }
            else if (course == null)
            {
                return new Response(404, false, "Not Found!");
            }
            else
            {
                course.IsFree = request.IsFree;
                course.Description = request.Description;
                course.Title = request.Title;
                course.Price = request.Price;

                await _courseRepository.UpdateAsync(course);
                return new Response(200, true, "Курс успешно обновлен!");
            }

        }
        public async Task<Response> DeleteCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return new Response(404, false, "Курс с таким айди не найден!");
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var parsedId);
            var seller = await _sellerRepository.GetByUserId(parsedId);
            if (seller.Id != course.SellerId)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
            }
            await _courseRepository.DeleteAsync(id);
            return new Response(200, true, "Вы успешно удалили данный курс!");
        }
        public async Task<Response> AddCourseToStudent(int courseId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
        public async Task<Response> RemoveCourseFromStudent(int courseId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
        public async Task<List<CourseDTO>> GetAllCourses([FromQuery] GetAllCoursesQuery query)
        {
            var courses = _context.Courses
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();
            var coursesdto = new List<CourseDTO>();
            foreach (var course in courses)
            {
                coursesdto.Add(new CourseDTO 
                { 
                    Description = course.Description,
                    IsFree = course.IsFree,
                    Price = course.Price,
                    Title = course.Title,
                    SellerId = course.SellerId,
                    Id = course.Id,
                });
            }
            return coursesdto;
        }
        public async Task<List<CourseDTO>> SearchCourses([FromQuery] SearchCoursesQuery query)
        {
            var courses = _context.Courses
            .Where(c => c.Title.Contains(query.Word) || c.Description.Contains(query.Word))
            .OrderBy(c => c.Title)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize);
            var coursesdto = new List<CourseDTO>();
            foreach (var course in courses)
            {
                coursesdto.Add(new CourseDTO
                {
                    Description = course.Description,
                    IsFree = course.IsFree,
                    Price = course.Price,
                    Title = course.Title,
                    SellerId = course.SellerId,
                    Id = course.Id,
                });
            }
            return coursesdto;
        }
    }
}
