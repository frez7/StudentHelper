using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentHelper.BL.Services.CourseServices;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace StudentHelper.BL.Services.OtherServices
{
    public class ValidationService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly GetService _getService;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<VideoLesson> _videoRepository;
        private readonly IRepository<Test> _testRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly CourseContext _context;
        public ValidationService(IRepository<Student> studentRepository, GetService getService,
            IRepository<Course> courseRepository, IRepository<Seller> sellerRepository, IRepository<Page> pageRepository,
            IRepository<VideoLesson> videoRepository, IRepository<Test> testRepository, IRepository<Question> questionRepository,
            IRepository<Answer> answerRepository, CourseContext context)
        {
            _studentRepository = studentRepository;
            _getService = getService;
            _courseRepository = courseRepository;
            _sellerRepository = sellerRepository;
            _pageRepository = pageRepository;
            _videoRepository = videoRepository;
            _testRepository = testRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _context = context;
        }
        public async Task<bool> CheckCourseRelation(int courseId)
        {
            var sortedStudentIds = _context.StudentCourses
                .Where(sc => sc.CourseId == courseId)
                .OrderBy(sc => sc.CourseId)
                .Select(sc => sc.StudentId)
                .ToList();
            var userId = _getService.GetCurrentUserId();
            var student = await _studentRepository.GetByUserId(userId);
            if (!sortedStudentIds.Contains(student.Id))
            {
                return false;
            }
            return true;
        }
        public async Task<bool> GetCourseOwner(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new Exception("Курса с таким id, не существует!");
            }
            var parsedId = _getService.GetCurrentUserId();
            var seller = await _sellerRepository.GetByUserId(parsedId);
            if (seller == null)
            {
                throw new Exception("Вы не являетесь продавцом!");
            }
            if (seller.Id != course.SellerId)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> GetPageOwner(int pageId)
        {
            var page = await _pageRepository.GetByIdAsync(pageId);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            var result = await GetCourseOwner(course.Id);
            if (result == true)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> GetTestOwner(int testId)
        {
            var test = await _testRepository.GetByIdAsync(testId);
            var page = await _pageRepository.GetByIdAsync(test.PageId);
            var result = await GetPageOwner(page.Id);
            if (result == true)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> GetQuestionOwner(int questionId)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);
            var test = await _testRepository.GetByIdAsync(question.TestId);
            var result = await GetTestOwner(test.Id);
            if (result == true)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> GetAnswerOwner(int answerId)
        {
            var answer = await _answerRepository.GetByIdAsync(answerId);
            var question = await _questionRepository.GetByIdAsync(answer.QuestionId);
            var result = await GetQuestionOwner(question.Id);
            if (result == true)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> GetVideoLessonOwner(int videoId)
        {
            var video = await _videoRepository.GetByIdAsync(videoId);
            var page = await _pageRepository.GetByIdAsync(video.PageId);
            var result = await GetPageOwner(page.Id);
            if (result == true)
            {
                return true;
            }
            return false;
        }
    }
}
