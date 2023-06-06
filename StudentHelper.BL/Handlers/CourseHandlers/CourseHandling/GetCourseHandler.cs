using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.CourseQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class GetCourseHandler : IRequestHandler<GetCourseQuery, CourseDTO>
    {
        private readonly CourseService _service;
        public GetCourseHandler(CourseService service)
        {
            _service = service;
        }
        public async Task<CourseDTO> Handle(GetCourseQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetCourse(query.Id);
        }
    }
}
