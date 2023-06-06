using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.CourseQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class SearchCoursesHandler : IRequestHandler<SearchCoursesQuery, List<CourseDTO>>
    {
        private readonly CourseService _service;
        public SearchCoursesHandler(CourseService service)
        {
            _service = service;
        }

        public async Task<List<CourseDTO>> Handle(SearchCoursesQuery query, CancellationToken cancellationToken)
        {
            return await _service.SearchCourses(query);
        }
    }
}
