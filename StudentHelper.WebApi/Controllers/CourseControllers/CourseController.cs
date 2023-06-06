using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Queries.CourseQueries;
using StudentHelper.Model.Models.Requests.CourseRequests;


namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create-course")]
        public async Task<Response> CreateCourse([FromBody]CreateCourseRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpGet("course/{id}")]
        public async Task<CourseDTO> GetCourse(int id)
        {
            return await _mediator.Send(new GetCourseQuery { Id = id });
        }
        [HttpPut("courses/{id}/image")]
        public async Task<Response> UpdateCourseImage(int id, IFormFile image)
        {
            return await _mediator.Send(new UpdateCourseImageQuery { Id = id, Image = image });
        }
        [HttpGet("course/{courseId}/image")]
        public async Task<IActionResult> GetImage(int courseId)
        {
            return (IActionResult)await _mediator.Send(new GetImageQuery { CourseId = courseId });
        }
        [HttpPut("courses/{id}")]
        public async Task<Response> UpdateCourse([FromBody] UpdateCourseRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpDelete("delete-course/{id}")]
        public async Task<Response> DeleteCourse(int id)
        {
            return await _mediator.Send(new DeleteCourseQuery { Id = id});
        }
        [HttpPost("remove-course-from-student")]
        public async Task<Response> RemoveCourseFromStudent(int courseId)
        {
            return await _mediator.Send(new RemoveCourseFromStudentQuery { CourseId = courseId });
        }
        
        [HttpGet("courses/{courseId}/students")]
        public async Task<List<ApplicationUserDTO>> GetAllCourseStudents(int courseId)
        {
            return await _mediator.Send(new GetAllCourseStudentsQuery { CourseId = courseId });
        }

        [HttpGet("students/{studentId}/courses")]
        public async Task<List<Course>> GetAllStudentCourses(int studentId)
        {
            return await _mediator.Send(new GetAllStudentCoursesQuery { StudentId = studentId });
        }
        
        [HttpGet("courses")]
        public async Task<List<CourseDTO>> GetAllCourses(int pageNumber, int pageSize)
        {
            return await _mediator.Send(new GetAllCoursesQuery { PageNumber = pageNumber, PageSize = pageSize});
        }
        [HttpGet("courses/search")]
        public async Task<List<CourseDTO>> SearchCourses(int pageNumber, int pageSize, string word)
        {
            return await _mediator.Send(new SearchCoursesQuery { PageNumber = pageNumber, PageSize = pageSize, Word = word});
        }
    }
}
