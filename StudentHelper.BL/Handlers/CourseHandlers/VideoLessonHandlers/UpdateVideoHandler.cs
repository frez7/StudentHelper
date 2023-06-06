using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests.VideoRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.VideoLessonHandlers
{
    public class UpdateVideoHandler : IRequestHandler<UpdateVideoLessonRequest, VideoResponse>
    {
        private readonly VideoService _service;
        public UpdateVideoHandler(VideoService service)
        {
            _service = service;
        }

        public async Task<VideoResponse> Handle(UpdateVideoLessonRequest request, CancellationToken cancellationToken)
        {
            return await _service.UpdateVideo(request);
        }
    }
}
