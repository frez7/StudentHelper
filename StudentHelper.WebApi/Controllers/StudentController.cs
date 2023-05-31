using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests;
using System.Security.Claims;

namespace StudentHelper.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Student> _repository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly CourseService _courseService;

        public StudentController(IHttpContextAccessor httpContextAccessor, IRepository<Student> repository,
            IRepository<Course> courseRepository, IRepository<Seller> sellerRepository, CourseService courseService)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _courseRepository = courseRepository;
            _sellerRepository = sellerRepository;
            _courseService = courseService;
        }

        [HttpPost("IncreaseMoneyBalance")]
        public async Task<IncreaseMoneyBalanceResponse> IncreaseMoneyBalance(int money)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var student = await _repository.GetByUserId(id);

            student.MoneyBalance += money;
            await _repository.UpdateAsync(student);


            return new IncreaseMoneyBalanceResponse(200, true, "Ваш баланс пополнен!", money);
        }

        [HttpGet("Balance")]
        public async Task<IncreaseMoneyBalanceResponse> GetBalance()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var student = await _repository.GetByUserId(id);

            return new IncreaseMoneyBalanceResponse(200, true, "Информация о балансе получена.", student.MoneyBalance);
        }

        [HttpPost("PaymentForCourse")]
        public async Task<Response> PaymentForCourse(int courseId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var student = await _repository.GetByUserId(id);

            //вычисляю курс по айди и беру его прайс
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new Exception("Курс не найден!");
            }

            var seller = await _sellerRepository.GetByIdAsync(course.SellerId);

            var studentBalance = student.MoneyBalance;

            var coursePrice = course.Price;

            if (studentBalance >= coursePrice)
            {
                // Вычитаем стоимость курса из баланса студента
                student.MoneyBalance -= coursePrice;

                // Добавляем курс к студенту
                await _courseService.AddCourseToStudent(courseId);
                // Прибавляем стоимость курса к балансу продавца
                seller.MoneyBalance += coursePrice;

                // Сохраняем изменения
                await _repository.UpdateAsync(student);
                await _sellerRepository.UpdateAsync(seller);
                await _courseRepository.UpdateAsync(course); 

                return new Response(200, true, "Курс успешно приобретен!");
            }
            else
            {
                return new Response(400, false, "У вас недостаточно средств для приобретения курса.");
            }


        }


    }




}






