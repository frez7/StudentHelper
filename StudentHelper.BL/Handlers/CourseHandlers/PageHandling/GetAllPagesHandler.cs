using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.PageQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.PageHandling
{
    public class GetAllPagesHandler : IRequestHandler<GetAllPagesQuery, List<PageDTO>>
    {
        private readonly PageService _service;
        public GetAllPagesHandler(PageService service)
        {
            _service = service;
        }

        public async Task<List<PageDTO>> Handle(GetAllPagesQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetAllPagesByCourseId(query.CourseId);
        }
    }
}
