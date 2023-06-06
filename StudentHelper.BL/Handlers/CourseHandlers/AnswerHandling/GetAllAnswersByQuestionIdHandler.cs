using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.AnswerQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.AnswerHandling
{
    public class GetAllAnswersByQuestionIdHandler : IRequestHandler<GetAllAnswersByQuestionIdQuery, List<AnswerDTO>>
    {
        private readonly AnswerService _service;
        public GetAllAnswersByQuestionIdHandler(AnswerService service)
        {
            _service = service;
        }

        public async Task<List<AnswerDTO>> Handle(GetAllAnswersByQuestionIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetAllAnswersByQuestionId(query.Id);
        }
    }
}
