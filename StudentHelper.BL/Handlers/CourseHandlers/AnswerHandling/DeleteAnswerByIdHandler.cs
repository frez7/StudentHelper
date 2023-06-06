using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.AnswerQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.AnswerHandling
{
    public class DeleteAnswerByIdHandler : IRequestHandler<DeleteAnswerByIdQuery, Response>
    {
        private readonly AnswerService _service;
        public DeleteAnswerByIdHandler(AnswerService service)
        {
            _service = service;
        }

        public async Task<Response> Handle(DeleteAnswerByIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.DeleteAnswerById(query.Id);
        }
    }
}
