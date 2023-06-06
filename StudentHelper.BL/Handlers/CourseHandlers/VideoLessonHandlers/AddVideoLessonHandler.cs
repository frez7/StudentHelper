using MediatR;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Requests.CourseRequests.VideoRequests;

namespace StudentHelper.BL.Handlers.CourseHandlers.VideoLessonHandlers
{
    public class AddVideoLessonHandler : IRequestHandler<AddVideoLessonRequest, VideoResponse>
    {
        private readonly VideoService _service;
        public AddVideoLessonHandler(VideoService service)
        {
            _service = service;
        }

        public async Task<VideoResponse> Handle(AddVideoLessonRequest request, CancellationToken cancellationToken)
        {
            return await _service.AddVideoLesson(request);
        }
    }
}
