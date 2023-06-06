using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.CourseQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.CourseHandling
{
    public class UpdateCourseImageHandler : IRequestHandler<UpdateCourseImageQuery, Response>
    {
        private readonly CourseService _service;
        public UpdateCourseImageHandler(CourseService service)
        {
            _service = service;
        }

        public async Task<Response> Handle([FromQuery]UpdateCourseImageQuery query, CancellationToken cancellationToken)
        {
            return await _service.UpdateCourseImage(query.Id, query.Image);
        }
    }
}
