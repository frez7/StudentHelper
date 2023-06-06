using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.CourseQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class RemoveCourseFromStudentHandler : IRequestHandler<RemoveCourseFromStudentQuery, Response>
    {
        private readonly CourseService _service;
        public RemoveCourseFromStudentHandler(CourseService service)
        {
            _service = service;
        }

        public async Task<Response> Handle(RemoveCourseFromStudentQuery query, CancellationToken cancellationToken)
        {
            return await _service.RemoveCourseFromStudent(query.CourseId);
        }
    }
}
