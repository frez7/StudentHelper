using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.CourseRequests;
using System.Security.Claims;

namespace StudentHelper.WebApi.Controllers.CourseControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PageController : ControllerBase
    {
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Course> _courseRepository;

        public PageController(IRepository<Page> pageRepository, IRepository<Seller> sellerRepository, IRepository<Course> courseRepository)
        {
            _pageRepository = pageRepository;
            _sellerRepository = sellerRepository;
            _courseRepository = courseRepository;
        }
        [HttpPost("course/page/create")]
        public async Task<Response> CreatePage([FromBody] CreatePageRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userId, out var id);
            Seller seller = await _sellerRepository.GetByUserId(id);
            var course = await _courseRepository.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                return new Response(400, false, "Курс с таким айди не найден!");
            }
            if (seller == null)
            {
                return new Response(400, false, "Ты не являешься продавцом!");
            }
            if (seller.Id != course.SellerId)
            {
                return new Response(400, false, "Этот курс не пренадлежит тебе, ты не можешь добавлять в него страницы!");
            }
            var page = new Page
            {
                Title = request.Title,
                Description = request.Description,
                Content = request.Content,
                CourseId = request.CourseId,
            };
            return new Response(200, true, "Страница добавлена!");
        }
    }
}
