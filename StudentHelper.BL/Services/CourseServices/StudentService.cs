using Microsoft.AspNetCore.Http;
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
using StudentHelper.BL.Services.OtherServices;
using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.BL.Services.CourseServices
{
    public class StudentService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly CourseContext _context;
        private readonly CourseService _courseService;
        private readonly EnrollmentService _enrollmentService;
        private readonly GetService _getService;
        private readonly IRepository<FavouriteCourse> _favouriteCourseRepository;

        public StudentService(IRepository<Course> courseRepository, IRepository<Student> studentRepository, IRepository<Seller> sellerRepository,
            CourseContext context, IHttpContextAccessor httpContextAccessor, CourseService courseService,
            EnrollmentService enrollmentService, GetService getService, IRepository<FavouriteCourse> favouriteCourseRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _sellerRepository = sellerRepository;
            _context = context;
            _courseService = courseService;
            _enrollmentService = enrollmentService;
            _getService = getService;
            _favouriteCourseRepository = favouriteCourseRepository;
        }

        public async Task<IncreaseMoneyBalanceResponse> IncreaseMoneyBalance(int money)
        {
            var id = _getService.GetCurrentUserId();
            var student = await _studentRepository.GetByUserId(id);

            student.MoneyBalance += money;
            await _studentRepository.UpdateAsync(student);


            return new IncreaseMoneyBalanceResponse(200, true, "Ваш баланс пополнен!", money);
        }


        public async Task<IncreaseMoneyBalanceResponse> GetBalance()
        {
            var id = _getService.GetCurrentUserId();
            var student = await _studentRepository.GetByUserId(id);

            return new IncreaseMoneyBalanceResponse(200, true, "Информация о балансе получена.", student.MoneyBalance);
        }


        public async Task<Response> PaymentForCourse(int courseId)
        {
            var id = _getService.GetCurrentUserId();
            var student = await _studentRepository.GetByUserId(id);


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

        public async Task<Response> AddToFavourites(int courseId)
        {
            var id = _getService.GetCurrentUserId();

            var student = await _studentRepository.GetByUserId(id);
            var course = await _courseRepository.GetByIdAsync(courseId);

            if (course == null || student == null)
            {
                return new Response(404, false, "Одна из сущностей не найдена!");
            }

            var favouriteCourse = new FavouriteCourse
            {
                Student = student,
                Course = course,
            };

            await _favouriteCourseRepository.AddAsync(favouriteCourse);

            return new Response(200, true, "Вы успешно добавили курс к избранным!");

        }


        public async Task<Response> RemoveFromFavourites(int courseId)
        {
            var id = _getService.GetCurrentUserId();

            var student = await _studentRepository.GetByUserId(id);
            var favouriteCourse = _context.Favourites.FirstOrDefault(f => f.CourseId == courseId && f.StudentId == student.Id);
            if (favouriteCourse == null)
            {
                throw new Exception("Курс не найден!");
            }
            await _favouriteCourseRepository.RemoveAsync(favouriteCourse);

            return new Response(200, true, "Вы успешно удалили курс из избранных!");

        }


        public async Task<List<Course>> GetAllFavourites()
        {
            var userId = _getService.GetCurrentUserId();
            var student = await _studentRepository.GetByUserId(userId);
            var favouriteCourseIds = _context.Favourites
                .Where(sc => sc.StudentId == student.Id)
                .OrderBy(sc => sc.StudentId)
                .Select(sc => sc.CourseId)
                .ToList();

            var favouriteCourses = new List<Course>();
            for (int i = 0; i < favouriteCourseIds.Count; i++)
            {
                var favouriteCourse = await _courseRepository.GetByIdAsync(favouriteCourseIds[i]);
                favouriteCourses.Add(favouriteCourse);
            }

            return favouriteCourses;

        }
    } 
}
