using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.PageQueries;
using StudentHelper.Model.Models.Requests.CourseRequests.PageRequests;

namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PageController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("course/page/create")]
        public async Task<PageResponse> CreatePage([FromBody] CreatePageRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("course/{courseId}/pages")]
        public async Task<List<PageDTO>> GetAllPages(int courseId)
        {
            return await _mediator.Send(new GetAllPagesQuery { CourseId = courseId });
        }

        [HttpGet("course/page/{pageId}")]
        public async Task<PageDTOResponse> GetPage(int pageId)
        {
            return await _mediator.Send(new GetPageQuery { PageId = pageId });
        }

        [HttpPost("course/page/update")]
        public async Task<PageResponse> UpdatePage([FromBody] UpdatePageRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpDelete("/page/delete/{pageId}")]
        public async Task<Response> DeletePage(int pageId)
        {
            return await _mediator.Send(new DeletePageQuery { PageId = pageId });
        }
        
        
    }
}
