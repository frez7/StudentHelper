using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models;
using StudentHelper.Model.Models.Queries.CourseQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class GetAllCourseStudentsHandler : IRequestHandler<GetAllCourseStudentsQuery, List<ApplicationUserDTO>>
    {
        private readonly CourseService _service;
        public GetAllCourseStudentsHandler(CourseService service)
        {
            _service = service;
        }

        public async Task<List<ApplicationUserDTO>> Handle(GetAllCourseStudentsQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetAllCourseStudents(query.CourseId);
        }
    }
}
