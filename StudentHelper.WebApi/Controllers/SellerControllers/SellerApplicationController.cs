using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.SellerRequests;

namespace StudentHelper.WebApi.Controllers.SellerControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SellerApplicationController : ControllerBase
    {
        private readonly SellerApplicationService _service;

        public SellerApplicationController(SellerApplicationService service)
        {
            _service = service;
        }

        [HttpGet("seller-app/{applicationId}")]
        public async Task<SellerApplication> GetById(int applicationId)
        {
            return await _service.GetById(applicationId);
        }

        [HttpPost("create-seller-application")]
        public async Task<Response> CreateSellerApplication(SellerApplicationRequest request)
        {
            return await _service.CreateSellerApplication(request);
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
            return await _service.Approve(id);
        }

        [HttpPost("{id}/reject")]
        public async Task<Response> Reject(int id)
        {
            return await _service.Reject(id);
        }
    }
}
