using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.CourseQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class GetAllCoursesHandler : IRequestHandler<GetAllCoursesQuery, List<CourseDTO>>
    {
        private readonly CourseService _service;
        public GetAllCoursesHandler(CourseService service)
        {
            _service = service;
        }
        public async Task<List<CourseDTO>> Handle(GetAllCoursesQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetAllCourses(query);
        }
    }
}
