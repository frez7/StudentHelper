using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Requests.CourseRequests;
using StudentHelper.Model.Models.Requests.CourseRequests.TestRequests;

namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestService _service;

        public TestController(TestService service)
        {
            _service = service;
        }

        [HttpPost("create-test")]
        public async Task<TestResponse> CreateTest(CreateTestRequest request)
        {
            return await _service.CreateTest(request);
        }

        [HttpPut("update-test")]
        public async Task<TestResponse> UpdateTest(UpdateTestRequest request)
        {
            return await _service.UpdateTest(request);
        }

        [HttpDelete("delete-test")]
        public async Task<Response> DeleteTest(int testId)
        {
            return await _service.DeleteTest(testId);
        }

        [HttpGet("get-all-tests-by-page-id")]
        public async Task<List<TestDTO>> GetAllTestsByPageId(int pageId)
        {
            return await _service.GetAllTestsByPageId(pageId);
        }

        [HttpGet("get-test")]
        public async Task<TestDTOResponse> GetTestById(int testId)
        {
            return await _service.GetTestById(testId);
        }
    }
}
