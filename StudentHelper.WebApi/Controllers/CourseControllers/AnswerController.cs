using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Queries.AnswerQueries;
using StudentHelper.Model.Models.Requests.CourseRequests.AnswerRequests;

namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AnswerController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("Create-answer")]
        public async Task<AnswerResponse> CreateAnswer(CreateAnswerRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("update-answer")]
        public async Task<AnswerResponse> UpdateAnswer(UpdateAnswerRequest request)
        {
            return await _mediator.Send(request);
        }


        [HttpDelete("delete-answer")]
        public async Task<Response> DeleteAnswerById(int answerId)
        {
            return await _mediator.Send(new DeleteAnswerByIdQuery { Id = answerId });
        }

        [HttpGet("get-all-answers-by id")]
        public async Task<List<AnswerDTO>> GetAllAnswersByQuestionId(int questionId)
        {
            return await _mediator.Send(new GetAllAnswersByQuestionIdQuery { Id = questionId });
        }

        [HttpGet("get-answer-by-id")]
        public async Task<AnswerDTOResponse> GetAnswerById(int answerId)
        {
            return await _mediator.Send(new GetAnswerByIdQuery { Id = answerId });
        }

    }
}