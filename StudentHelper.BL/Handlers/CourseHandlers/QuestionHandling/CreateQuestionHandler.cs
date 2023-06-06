using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests.QuestionRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.QuestionHandling
{
    public class CreateQuestionHandler : IRequestHandler<CreateQuestionRequest, QuestionResponse>
    {
        private readonly QuestionService _service;
        public CreateQuestionHandler(QuestionService service)
        {
            _service = service;
        }

        public async Task<QuestionResponse> Handle(CreateQuestionRequest request, CancellationToken cancellationToken)
        {
            return await _service.CreateQuestion(request);
        }
    }
}
