using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Requests.CourseRequests.QuestionRequests;

namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _service;
        public QuestionController(QuestionService service)
        {
            _service = service;
        }

        [HttpPost("Create-question")]
        public async Task<QuestionResponse> CreateQuestion(CreateQuestionRequest request)
        {
            return await _service.CreateQuestion(request);
        }

        [HttpPut("update-question")]
        public async Task<QuestionResponse> UpdateQuestion(UpdateQuestionRequest request)
        {
            return await _service.UpdateQuestion(request);
        }

        [HttpDelete("delete-question")]
        public async Task<Response> DeleteQuestionById(int questionId)
        {
            return await _service.DeleteQuestionById(questionId);
        }

        [HttpGet("get-all-questions-by id")]
        public async Task<List<QuestionDTO>> GetAllQuestionsByTestId(int testId)
        {
            return await _service.GetAllQuestionsByTestId(testId);
        }

        [HttpGet("get-question-by-id")]
        public async Task<QuestionDTOResponse> GetQuestionById(int questionId)
        {
            return await _service.GetQuestionById(questionId);
        }

    }
}
