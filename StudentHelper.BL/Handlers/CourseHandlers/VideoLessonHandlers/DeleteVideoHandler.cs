using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.VideoLessonQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.VideoLessonHandlers
{
    public class DeleteVideoHandler : IRequestHandler<DeleteVideoQuery, Response>
    {
        private readonly VideoService _service;
        public DeleteVideoHandler(VideoService service)
        {
            _service = service;
        }

        public async Task<Response> Handle(DeleteVideoQuery query, CancellationToken cancellationToken)
        {
            return await _service.DeleteVideo(query.VideoId);
        }
    }
}
