using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests.PageRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.PageHandling
{
    public class CreatePageHandler : IRequestHandler<CreatePageRequest, PageResponse>
    {
        private readonly PageService _service;
        public CreatePageHandler(PageService service)
        {
            _service = service;
        }

        public async Task<PageResponse> Handle(CreatePageRequest request, CancellationToken cancellationToken)
        {
            return await _service.CreatePage(request);
        }
    }
}
