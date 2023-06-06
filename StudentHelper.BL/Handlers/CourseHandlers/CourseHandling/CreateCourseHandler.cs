using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests;
using StudentHelper.Model.Models.Requests.CourseRequests.AnswerRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseRequest, CourseResponse>
    {
        private readonly CourseService _service;
        public CreateCourseHandler(CourseService service)
        {
            _service = service;
        }

        public async Task<CourseResponse> Handle(CreateCourseRequest request, CancellationToken cancellationToken)
        {
            return await _service.CreateCourse(request);
        }
    }
}
