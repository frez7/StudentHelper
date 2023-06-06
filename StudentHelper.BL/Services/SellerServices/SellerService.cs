using Microsoft.EntityFrameworkCore;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.BL.Services.OtherServices;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.BL.Services.SellerServices
{
    public class SellerService
    {
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly GetService _getService;
        private readonly CourseContext _context;
        public SellerService(IRepository<Seller> sellerRepository, GetService getService, CourseContext context, IRepository<Course> courseRepository)
        {
            _sellerRepository = sellerRepository;
            _getService = getService;
            _context = context;
            _courseRepository = courseRepository;
        }

        public async Task<List<Seller>> GetSellersAsync()
        {
            var sellers = await _sellerRepository.GetAllAsync();
            return sellers.ToList();
        }
        
        public async Task<List<CourseDTO>> GetAllSellerCourses()
        {
            var userId = _getService.GetCurrentUserId();
            var seller = await _sellerRepository.GetByUserId(userId);
            var sortedCourseIds = _context.Courses
                .Where(sc => sc.SellerId == seller.Id)
                .OrderBy(sc => sc.SellerId)
                .Select(sc => sc.Id)
                .ToList();
            var coursesDTO = new List<CourseDTO>();
            for (int i = 0; i < sortedCourseIds.Count; i++)
            {
                var course = await _courseRepository.GetByIdAsync(sortedCourseIds[i]);
                var courseDTO = new CourseDTO
                {
                    Id = course.Id,
                    Description = course.Description,
                    IsFree = course.IsFree,
                    Price = course.Price,
                    SellerId = course.SellerId,
                    Title = course.Title,
                };
                coursesDTO.Add(courseDTO);
            }
            return coursesDTO;
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
