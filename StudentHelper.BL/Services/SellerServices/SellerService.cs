using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.BL.Services.SellerServices
{
    public class SellerService
    {
        private readonly IRepository<Seller> _sellerRepository;

        public SellerService(IRepository<Seller> sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }

        public async Task<List<Seller>> GetSellersAsync()
        {
            var sellers = await _sellerRepository.GetAllAsync();
            return sellers.ToList();
        }
    }
}
