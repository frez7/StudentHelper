using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.VideoLessonQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.VideoLessonHandlers
{
    public class GetVideoByIdHandler : IRequestHandler<GetVideoByIdQuery, VideoLessonDTO>
    {
        private readonly VideoService _service;
        public GetVideoByIdHandler(VideoService service)
        {
            _service = service;
        }

        public async Task<VideoLessonDTO> Handle(GetVideoByIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetVideoById(query.VideoId);
        }
    }
}
