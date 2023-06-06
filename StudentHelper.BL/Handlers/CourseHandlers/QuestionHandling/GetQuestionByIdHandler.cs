using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Queries.QuestionQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.QuestionHandling
{
    public class GetQuestionByIdHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDTOResponse>
    {
        private readonly QuestionService _service;
        public GetQuestionByIdHandler(QuestionService service)
        {
            _service = service;
        }

        public async Task<QuestionDTOResponse> Handle(GetQuestionByIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetQuestionById(query.Id);
        }
    }
}
