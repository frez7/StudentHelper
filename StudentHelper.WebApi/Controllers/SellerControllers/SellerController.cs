using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.WebApi.Controllers.SellerControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly IRepository<Seller> _sellerRepository;

        public SellerController(IRepository<Seller> sellerRepository) 
        { 
            _sellerRepository = sellerRepository;
        }
        [HttpGet("sellers")]
        public async Task<List<Seller>> GetSellersAsync()
        {
            var sellers = await _sellerRepository.GetAllAsync();
            return sellers.ToList();
        }
    }
}
