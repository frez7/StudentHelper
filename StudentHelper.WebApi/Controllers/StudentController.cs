using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Requests;

namespace StudentHelper.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IRepository<Student> _repository;
        private readonly ICourseRepository<Course> _courseRepository;

        [HttpPost("IncreaseMoneyBalance")]
        public async Task<IncreaseMoneyBalanceResponse> IncreaseMoneyBalance(IncreaseMoneyBalanceRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _repository.GetByUserId(user.Id);

            if (user == null)
            {
                return new IncreaseMoneyBalanceResponse(400, false, "Пользователь не найден.", 0);
            }


            student.MoneyBalance += request.MoneyAmount;
            await _repository.UpdateAsync(student);


            return new IncreaseMoneyBalanceResponse(200, true, "Ваш баланс пополнен на ", request.MoneyAmount);
        }

        [HttpGet("Balance")]
        public async Task<IncreaseMoneyBalanceResponse> GetBalance()
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _repository.GetByUserId(user.Id);

            if (student == null)
            {
                return new IncreaseMoneyBalanceResponse(400, false, "Пользователь не найден.", 0);
            }

            return new IncreaseMoneyBalanceResponse(200, true, "Информация о балансе получена.", student.MoneyBalance);
        }

        [HttpPost("PaymentForCourse")]
        public async Task<Response> PaymentForCourse(PaymentForCourseRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _repository.GetByUserId(user.Id);

            //вычисляю курс по айди и беру его прайс
            var course = await _courseRepository.GetCourseByIdAsync(user.Id);

            if (course == null)
            {
                throw new Exception("Курс не найден");
            }

            var studentBalance = student.MoneyBalance;

            var coursePrice = course.Price;

            if (studentBalance >= coursePrice)
            {
                // Вычитаем стоимость курса из баланса студента
                student.MoneyBalance -= coursePrice;

                // Добавляем студента в список студентов курса
                course.Students.Add(new StudentCourse { StudentId = student.Id, CourseId = course.Id });

                // Прибавляем стоимость курса к балансу продавца
                course.Seller.MoneyBalance += coursePrice;

                // Сохраняем изменения
                await _repository.UpdateAsync(student);
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






