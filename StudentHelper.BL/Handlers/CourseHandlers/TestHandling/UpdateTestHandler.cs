using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests.TestRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.TestHandling
{
    public class UpdateTestHandler : IRequestHandler<UpdateTestRequest, TestResponse>
    {
        private readonly TestService _service;
        public UpdateTestHandler(TestService service)
        {
            _service = service;
        }

        public async Task<TestResponse> Handle(UpdateTestRequest request, CancellationToken cancellationToken)
        {
            return await _service.UpdateTest(request);
        }
    }
}
