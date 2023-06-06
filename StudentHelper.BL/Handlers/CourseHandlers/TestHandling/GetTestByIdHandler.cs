using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Queries.TestQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.TestHandling
{
    public class GetTestByIdHandler : IRequestHandler<GetTestByIdQuery, TestDTOResponse>
    {
        private readonly TestService _service;
        public GetTestByIdHandler(TestService service)
        {
            _service = service;
        }

        public async Task<TestDTOResponse> Handle(GetTestByIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetTestById(query.TestId);
        }
    }
}
