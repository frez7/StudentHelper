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
using StudentHelper.Model.Models.Requests.CourseRequests;
using System.Security.Claims;

namespace StudentHelper.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("IncreaseMoneyBalance")]
        public async Task<IncreaseMoneyBalanceResponse> IncreaseMoneyBalance(int money)
        {
            return await _studentService.IncreaseMoneyBalance(money);
        }

        [HttpGet("Balance")]
        public async Task<IncreaseMoneyBalanceResponse> GetBalance()
        {
            return await _studentService.GetBalance();
        }

        [HttpPost("PaymentForCourse")]
        public async Task<Response> PaymentForCourse(int courseId)
        {
            return await _studentService.PaymentForCourse(courseId);

        }

        [HttpPost("AddToFavourites")]
        public async Task<Response> AddToFavourites(int courseId)
        {
            return await _studentService.AddToFavourites(courseId);

        }

        [HttpGet("GetFavourites")]
        public async Task<List<Course>> GetAllFavourites(int studentId)
        {
            return await _studentService.GetAllFavourites(studentId);

        }

        [HttpPost("RemoveFromFavourites")]
        public async Task<Response> RemoveFromFavourites(int courseId)
        {
            return await _studentService.RemoveFromFavourites(courseId);

        }



    }




}






