using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.VideoLessonQueries;
using StudentHelper.Model.Models.Requests.CourseRequests.VideoRequests;

namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VideoLessonController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VideoLessonController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("video/add")]
        public async Task<VideoResponse> AddVideoLesson([FromBody] AddVideoLessonRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpGet("video/{videoId}")]
        public async Task<VideoLessonDTO> GetVideoById(int videoId)
        {
            return await _mediator.Send(new GetVideoByIdQuery { VideoId = videoId });
        }
        [HttpGet("page/{pageId}/videos")]
        public async Task<List<VideoLessonDTO>> GetVideosByPageId(int pageId)
        {
            return await _mediator.Send(new GetVideosByPageIdQuery { PageId = pageId });
        }
        [HttpPut("video/{videoId}/update")]
        public async Task<VideoResponse> UpdateVideo([FromBody] UpdateVideoLessonRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpDelete("video/{videoId}/delete")]
        public async Task<Response> DeleteVideo(int videoId)
        {
            return await _mediator.Send(new DeleteVideoQuery { VideoId = videoId });
        }
    }
}
