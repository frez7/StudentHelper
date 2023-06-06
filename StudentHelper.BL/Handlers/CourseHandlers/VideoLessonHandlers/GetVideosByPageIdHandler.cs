using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.VideoLessonQueries;

namespace StudentHelper.BL.Handlers.CourseHandlers.VideoLessonHandlers
{
    public class GetVideosByPageIdHandler : IRequestHandler<GetVideosByPageIdQuery, List<VideoLessonDTO>>
    {
        private readonly VideoService _service;
        public GetVideosByPageIdHandler(VideoService service)
        {
            _service = service;
        }

        public async Task<List<VideoLessonDTO>> Handle(GetVideosByPageIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetVideosByPageId(query.PageId);
        }
    }
}
