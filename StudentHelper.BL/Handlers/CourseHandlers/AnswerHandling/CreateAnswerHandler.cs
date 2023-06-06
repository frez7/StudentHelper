using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests.AnswerRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.AnswerHandling
{
    public class CreateAnswerHandler : IRequestHandler<CreateAnswerRequest, AnswerResponse>
    {
        private readonly AnswerService _service;
        public CreateAnswerHandler(AnswerService service)
        {
            _service = service;
        }

        public async Task<AnswerResponse> Handle(CreateAnswerRequest request, CancellationToken cancellationToken)
        {
            return await _service.CreateAnswer(request);
        }
    }
}
