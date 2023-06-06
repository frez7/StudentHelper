using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.PageQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.PageHandling
{
    public class DeletePageHandler : IRequestHandler<DeletePageQuery, Response>
    {
        private readonly PageService _service;
        public DeletePageHandler(PageService service)
        {
            _service = service;
        }

        public async Task<Response> Handle(DeletePageQuery query, CancellationToken cancellationToken)
        {
            return await _service.DeletePageById(query.PageId);
        }
    }
}
