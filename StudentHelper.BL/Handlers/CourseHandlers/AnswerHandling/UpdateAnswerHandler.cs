using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests.AnswerRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.AnswerHandling
{
    public class UpdateAnswerHandler : IRequestHandler<UpdateAnswerRequest, AnswerResponse>
    {
        private readonly AnswerService _service;
        public UpdateAnswerHandler(AnswerService service)
        {
            _service = service;
        }

        public async Task<AnswerResponse> Handle(UpdateAnswerRequest request, CancellationToken cancellationToken)
        {
            return await _service.UpdateAnswer(request);
        }
    }
}
