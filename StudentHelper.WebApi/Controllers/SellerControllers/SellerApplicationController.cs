using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.SellerResponses;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Queries.SellerApplicationQueries;
using StudentHelper.Model.Models.Requests.SellerRequests;

namespace StudentHelper.WebApi.Controllers.SellerControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SellerApplicationController : ControllerBase
    {
        private readonly SellerApplicationService _service;
        private readonly IMediator _mediator;

        public SellerApplicationController(SellerApplicationService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        [HttpGet("seller-app/{applicationId}")]
        public async Task<SellerApplication> GetById(int applicationId)
        {
            return await _mediator.Send(new GetByIdQuery { Id = applicationId });
        }

        [HttpPost("create-seller-application")]
        public async Task<Response> CreateSellerApplication(SellerApplicationRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("status")]
        public async Task<SellerApplicationResponse> GetSelfStatus()
        {
            return await _service.GetSelfStatus();
        }

        [HttpGet("get-all-applications")]
        public async Task<List<SellerApplication>> GetAllSellerApplications()
        {
            return await _service.GetAllSellerApplications();
        }

        [HttpPost("{id}/approve")]
        public async Task<Response> Approve(int id)
        {
            return await _mediator.Send(new ApproveQuery { Id = id });
        }

        [HttpPost("{id}/reject")]
        public async Task<Response> Reject(int id)
        {
            return await _mediator.Send(new RejectQuery { Id = id });
        }
    }
}
