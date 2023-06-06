using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.QuestionQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.QuestionHandling
{
    public class DeleteQuestionByIdHandler : IRequestHandler<DeleteQuestionByIdQuery, Response>
    {
        private readonly QuestionService _service;
        public DeleteQuestionByIdHandler(QuestionService service)
        {
            _service = service;
        }

        public async Task<Response> Handle(DeleteQuestionByIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.DeleteQuestionById(query.Id);
        }
    }
}
