using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.TestQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.TestHandling
{
    public class DeleteTestHandler : IRequestHandler<DeleteTestQuery, Response>
    {
        private readonly TestService _service;
        public DeleteTestHandler(TestService service)
        {
            _service = service;
        }

        public async Task<Response> Handle(DeleteTestQuery query, CancellationToken cancellationToken)
        {
            return await _service.DeleteTest(query.TestId);
        }
    }
}
