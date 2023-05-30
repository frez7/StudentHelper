using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.CourseRequests;
using System.Security.Claims;

namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VideoLessonController : ControllerBase
    {
        private readonly VideoService _videoService;
        public VideoLessonController(VideoService videoService)
        {
            _videoService = videoService;
        }
        [HttpPost("video/add")]
        public async Task<VideoResponse> AddVideoLesson([FromBody] AddVideoLessonRequest request)
        {
            return await _videoService.AddVideoLesson(request);
        }
        [HttpGet("video/{videoId}")]
        public async Task<VideoLessonDTO> GetVideoById(int videoId)
        {
            return await _videoService.GetVideoById(videoId);
        }
        [HttpGet("page/{pageId}/videos")]
        public async Task<List<VideoLessonDTO>> GetVideosByPageId(int pageId)
        {
            return await _videoService.GetVideosByPageId(pageId);
        }
        [HttpPut("video/{videoId}/update")]
        public async Task<VideoResponse> UpdateVideo(int videoId, [FromBody] AddVideoLessonRequest request)
        {
            return await _videoService.UpdateVideo(videoId, request);
        }
        [HttpDelete("video/{videoId}/delete")]
        public async Task<Response> DeleteVideo(int videoId)
        {
            return await _videoService.DeleteVideo(videoId);
        }
    }
}
