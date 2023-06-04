using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Entities.CourseDTOs;
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

        public async Task<SellerDTO> GetSellerById(int id)
        {
            var seller = await _sellerRepository.GetByIdAsync(id);
            if (seller == null)
            {
                throw new Exception("Продавец с таким айди не найден!");
            }
            var sellerDTO = new SellerDTO
            {
                Id = seller.Id,
                FirstName = seller.FirstName,
                LastName = seller.LastName,
                Phone = seller.Phone,
                Email = seller.Email,
                MoneyBalance = seller.MoneyBalance,
                CompanyName = seller.CompanyName,
                CompanyDescription = seller.CompanyDescription,
            };
            return sellerDTO;
        }
    }
}
