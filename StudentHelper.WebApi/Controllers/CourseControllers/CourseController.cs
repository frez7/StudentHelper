using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.CourseRequests;

namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IRepository<Course> _repository;

        public CourseController(IRepository<Course> repository)
        {
            _repository = repository;
        }
        [HttpPost("create-course")]
        public async Task<Response> CreateCourse(CreateCourseRequest request)
        {
            var course = new Course
            {
                Title = request.Title,
                Description = request.Description,
                IsFree = request.IsFree,
                Price = request.Price,
            };
            await _repository.AddAsync(course);
            return new Response(200, true, "Вы успешно создали курс!");
        }
        [HttpGet("{id}")]
        public async Task<Response> GetCourse(int id)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
            {
                return new Response(404, false, "Курс не найден!");
            }
            return new Response(200, true, $"{course.Title}");
        }
    }
}
