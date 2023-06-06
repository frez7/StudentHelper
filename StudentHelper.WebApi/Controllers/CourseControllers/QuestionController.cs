using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.QuestionQueries;
using StudentHelper.Model.Models.Requests.CourseRequests.QuestionRequests;

namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create-question")]
        public async Task<QuestionResponse> CreateQuestion(CreateQuestionRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("update-question")]
        public async Task<QuestionResponse> UpdateQuestion(UpdateQuestionRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpDelete("delete-question")]
        public async Task<Response> DeleteQuestionById(int questionId)
        {
            return await _mediator.Send(new DeleteQuestionByIdQuery { Id = questionId });
        }

        [HttpGet("get-all-questions-by id")]
        public async Task<List<QuestionDTO>> GetAllQuestionsByTestId(int testId)
        {
            return await _mediator.Send(new GetAllQuestionsByTestIdQuery {  Id = testId });
        }

        [HttpGet("get-question-by-id")]
        public async Task<QuestionDTOResponse> GetQuestionById(int questionId)
        {
            return await _mediator.Send(new GetQuestionByIdQuery { Id = questionId });
        }

    }
}
