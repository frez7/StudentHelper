using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.CourseQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourseQuery, Response>
    {
        private readonly CourseService _courseService;
        public DeleteCourseHandler(CourseService courseService)
        {
            _courseService = courseService;
        }

        public Task<Response> Handle(DeleteCourseQuery query, CancellationToken cancellationToken)
        {
            return _courseService.DeleteCourse(query.Id);
        }
    }
}
