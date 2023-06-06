using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Queries.CourseQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class GetAllStudentCoursesHandler : IRequestHandler<GetAllStudentCoursesQuery, List<Course>>
    {
        private readonly CourseService _service;
        public GetAllStudentCoursesHandler(CourseService service)
        {
            _service = service;
        }

        public async Task<List<Course>> Handle(GetAllStudentCoursesQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetAllStudentCourses(query.StudentId);
        }
    }
}
