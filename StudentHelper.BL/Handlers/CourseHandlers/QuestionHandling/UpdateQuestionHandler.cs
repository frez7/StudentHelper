using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests.QuestionRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.QuestionHandling
{
    public class UpdateQuestionHandler : IRequestHandler<UpdateQuestionRequest, QuestionResponse>
    {
        private readonly QuestionService _service;
        public UpdateQuestionHandler(QuestionService service)
        {
            _service = service;
        }

        public async Task<QuestionResponse> Handle(UpdateQuestionRequest request, CancellationToken cancellationToken)
        {
            return await _service.UpdateQuestion(request);
        }
    }
}
