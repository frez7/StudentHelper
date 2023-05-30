using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Requests.CourseRequests;


namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;

        public CourseController(CourseService service)
        {
            _service = service;
        }
        [HttpPost("create-course")]
        public async Task<Response> CreateCourse([FromBody]CreateCourseRequest request)
        {
            return await _service.CreateCourse(request);
        }
        [HttpGet("course/{id}")]
        public async Task<CourseDTO> GetCourse(int id)
        {
            return await _service.GetCourse(id);
        }
        [HttpPut("courses/{id}/image")]
        public async Task<Response> UpdateCourseImage(int id, IFormFile image)
        {
            return await _service.UpdateCourseImage(id, image);
        }
        [HttpGet("course/{courseId}/image")]
        public async Task<IActionResult> GetImage(int courseId)
        {
            return (IActionResult)await _service.GetImage(courseId);
        }
        [HttpPut("courses/{id}")]
        public async Task<Response> UpdateCourse(int id, [FromBody] CreateCourseRequest request)
        {
            return await _service.UpdateCourse(id, request);
        }
        [HttpDelete("delete-course/{id}")]
        public async Task<Response> DeleteCourse(int id)
        {
            return await _service.DeleteCourse(id);
        }
        [HttpPost("add-course-to-student")]
        public async Task<Response> AddCourseToStudent(int courseId)
        {
            return await _service.AddCourseToStudent(courseId);
        }
        [HttpPost("remove-course-from-student")]
        public async Task<Response> RemoveCourseFromStudent(int courseId)
        {
            return await _service.RemoveCourseFromStudent(courseId);
        }
        
        [HttpGet("courses/{courseId}/students")]
        public async Task<List<ApplicationUserDTO>> GetAllCourseStudents(int courseId)
        {
            return await _service.GetAllCourseStudents(courseId);
        }

        [HttpGet("students/{studentId}/courses")]
        public async Task<List<Course>> GetAllStudentCourses(int studentId)
        {
            return await _service.GetAllStudentCourses(studentId);
        }
        
        [HttpGet("courses")]
        public async Task<List<CourseDTO>> GetAllCourses([FromQuery] GetAllCoursesQuery query)
        {
            return await _service.GetAllCourses(query);
        }
        [HttpGet("courses/search")]
        public async Task<List<CourseDTO>> SearchCourses([FromQuery] SearchCoursesQuery query)
        {
            return await _service.SearchCourses(query);
        }
    }
}
