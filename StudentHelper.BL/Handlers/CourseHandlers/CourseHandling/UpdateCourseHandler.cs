using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class UpdateCourseHandler : IRequestHandler<UpdateCourseRequest, CourseResponse>
    {
        private readonly CourseService _courseService;

        public UpdateCourseHandler(CourseService courseService)
        {
            _courseService = courseService;
        }

        public Task<CourseResponse> Handle(UpdateCourseRequest request, CancellationToken cancellationToken)
        {
            return _courseService.UpdateCourse(request);
        }
    }
}
