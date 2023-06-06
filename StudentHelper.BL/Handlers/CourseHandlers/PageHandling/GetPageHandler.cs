using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Queries.PageQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.PageHandling
{
    public class GetPageHandler : IRequestHandler<GetPageQuery, PageDTOResponse>
    {
        private readonly PageService _service;
        public GetPageHandler(PageService service)
        {
            _service = service;
        }

        public async Task<PageDTOResponse> Handle(GetPageQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetPageById(query.PageId);
        }
    }
}
