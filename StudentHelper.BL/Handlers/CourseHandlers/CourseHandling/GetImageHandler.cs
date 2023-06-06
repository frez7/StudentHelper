using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Queries.CourseQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class GetImageHandler : IRequestHandler<GetImageQuery, IActionResult>
    {
        private readonly CourseService _service;
        public GetImageHandler(CourseService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Handle(GetImageQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetImage(query.CourseId);
        }
    }
}
