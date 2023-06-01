using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Requests.CourseRequests;

namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PageController : ControllerBase
    {
        private readonly PageService _pageService;
        public PageController(PageService pageService)
        {
            _pageService = pageService;
        }

        [HttpPost("course/page/create")]
        public async Task<Response> CreatePage([FromBody] CreatePageRequest request)
        {
            return await _pageService.CreatePage(request);
        }

        [HttpGet("course/{courseId}/pages")]
        public async Task<List<PageDTO>> GetAllPages(int courseId)
        {
            return await _pageService.GetAllPagesByCourseId(courseId);
        }

        [HttpGet("course/page/{pageId}")]
        public async Task<PageResponse> GetPage(int pageId)
        {
            return await _pageService.GetPageById(pageId);
        }

        [HttpPut("course/page/update")]
        public async Task<Response> UpdatePage([FromBody] UpdatePageRequest request)
        {
            return await _pageService.UpdatePage(request);
        }

        [HttpDelete("/page/delete/{pageId}")]
        public async Task<Response> DeletePage(int pageId)
        {
            return await _pageService.DeletePageById(pageId);
        }
        
        
    }
}
