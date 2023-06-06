using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.QuestionQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.QuestionHandling
{
    public class GetAllQuestionsByTestIdHandler : IRequestHandler<GetAllQuestionsByTestIdQuery, List<QuestionDTO>>
    {
        private readonly QuestionService _service;
        public GetAllQuestionsByTestIdHandler(QuestionService service)
        {
            _service = service;
        }

        public async Task<List<QuestionDTO>> Handle(GetAllQuestionsByTestIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetAllQuestionsByTestId(query.Id);
        }
    }
}
