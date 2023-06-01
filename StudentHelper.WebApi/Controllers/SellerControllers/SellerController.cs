using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Models.Common.SellerResponses;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.WebApi.Controllers.SellerControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly SellerService _sellerService;
        private readonly EnrollmentService _enrollmentService;

        public SellerController(SellerService sellerService, EnrollmentService enrollmentService)
        {
            _sellerService = sellerService;
            _enrollmentService = enrollmentService;
        }

        [HttpGet("sellers")]
        public async Task<List<Seller>> GetSellersAsync()
        {
            return await _sellerService.GetSellersAsync();
        }

        [HttpGet("seller/enrollments")]
        public async Task<EnrollmentsResponse> GetSellerEnrollments()
        {
            return await _enrollmentService.GetSellerEnrollments();
        }
    }
}
