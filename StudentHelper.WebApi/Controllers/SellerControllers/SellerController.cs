using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.WebApi.Controllers.SellerControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly SellerService _service;

        public SellerController(SellerService service)
        {
            _service = service;
        }

        [HttpGet("sellers")]
        public async Task<List<Seller>> GetSellersAsync()
        {
            return await _service.GetSellersAsync();
        }
    }
}
