using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests.TestRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.TestHandling
{
    public class CreateTestHandler : IRequestHandler<CreateTestRequest, TestResponse>
    {
        private readonly TestService _service;
        public CreateTestHandler(TestService service)
        {
            _service = service;
        }

        public async Task<TestResponse> Handle(CreateTestRequest request, CancellationToken cancellationToken)
        {
            return await _service.CreateTest(request);
        }
    }
}
