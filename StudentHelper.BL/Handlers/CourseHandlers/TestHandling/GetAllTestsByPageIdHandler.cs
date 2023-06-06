using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.TestQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.TestHandling
{
    public class GetAllTestsByPageIdHandler : IRequestHandler<GetAllTestsByPageIdQuery, List<TestDTO>>
    {
        private readonly TestService _service;
        public GetAllTestsByPageIdHandler(TestService service)
        {
            _service = service;
        }

        public async Task<List<TestDTO>> Handle(GetAllTestsByPageIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetAllTestsByPageId(query.PageId);
        }
    }
}
