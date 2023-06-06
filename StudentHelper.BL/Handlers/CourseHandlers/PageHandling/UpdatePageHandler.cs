using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests.PageRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.PageHandling
{
    public class UpdatePageHandler : IRequestHandler<UpdatePageRequest, PageResponse>
    {
        private readonly PageService _pageService;
        public UpdatePageHandler(PageService pageService)
        {
            _pageService = pageService;
        }

        public async Task<PageResponse> Handle(UpdatePageRequest request, CancellationToken cancellationToken)
        {
            return await _pageService.UpdatePage(request);
        }
    }
}
