using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.AnswerQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.AnswerHandling
{
    public class GetAnswerByIdHandler : IRequestHandler<GetAnswerByIdQuery, AnswerDTOResponse>
    {
        private readonly AnswerService _service;
        public GetAnswerByIdHandler(AnswerService service)
        {
            _service = service;
        }

        public async Task<AnswerDTOResponse> Handle(GetAnswerByIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetAnswerById(query.Id);
        }
    }
}
