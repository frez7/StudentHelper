﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Data;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentHelper.Model.Models.Common;
using System.Security.Claims;
using StudentHelper.BL.Services.SellerServices;

namespace StudentHelper.BL.Services.CourseServices
{
    public class StudentService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly CourseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CourseService _courseService;
        private readonly EnrollmentService _enrollmentService;

        public StudentService(IRepository<Course> courseRepository, IRepository<Student> studentRepository, IRepository<Seller> sellerRepository, 
            CourseContext context, IHttpContextAccessor httpContextAccessor, CourseService courseService,
            EnrollmentService enrollmentService)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _sellerRepository = sellerRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _courseService = courseService;
            _enrollmentService = enrollmentService;
        }

        public async Task<IncreaseMoneyBalanceResponse> IncreaseMoneyBalance(int money)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var student = await _studentRepository.GetByUserId(id);

            student.MoneyBalance += money;
            await _studentRepository.UpdateAsync(student);


            return new IncreaseMoneyBalanceResponse(200, true, "Ваш баланс пополнен!", money);
        }


        public async Task<IncreaseMoneyBalanceResponse> GetBalance()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var student = await _studentRepository.GetByUserId(id);

            return new IncreaseMoneyBalanceResponse(200, true, "Информация о балансе получена.", student.MoneyBalance);
        }


        public async Task<Response> PaymentForCourse(int courseId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var student = await _studentRepository.GetByUserId(id);

            //вычисляю курс по айди и беру его прайс
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new Exception("Курс не найден!");
            }
            var userSeller = await _sellerRepository.GetByUserId(id);
            var seller = await _sellerRepository.GetByIdAsync(course.SellerId);

            var studentBalance = student.MoneyBalance;

            var coursePrice = course.Price;
            if (userSeller.Id == seller.Id)
            {
                throw new Exception("Ты не можешь купить свой курс!");
            }

            if (studentBalance >= coursePrice)
            {
                student.MoneyBalance -= coursePrice;

                await _courseService.AddCourseToStudent(courseId);

                seller.MoneyBalance += coursePrice;

                await _studentRepository.UpdateAsync(student);
                await _sellerRepository.UpdateAsync(seller);
                await _courseRepository.UpdateAsync(course);

                await _enrollmentService.AddEnrollment(courseId);

                return new Response(200, true, "Курс успешно приобретен!");
            }
            else
            {
                return new Response(400, false, "У вас недостаточно средств для приобретения курса.");
            }


        }
























    }
}
