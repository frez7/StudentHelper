using Azure.Core;
using Microsoft.AspNetCore.Http;
using StudentHelper.BL.Services.OtherServices;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.CourseRequests.TestRequests;
using System.Security.Claims;

namespace StudentHelper.BL.Services.CourseServices
{
    public class TestService
    {
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<Test> _testRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CourseContext _dbContext;
        private readonly GetService _getService;
        private readonly ValidationService _validationService;

        public TestService(IRepository<Answer> answerRepository, IRepository<Question> questionRepository, IRepository<Seller> sellerRepository, 
            IRepository<Course> courseRepository, IRepository<Student> studentRepository, IRepository<Page> pageRepository,
            IRepository<Test> testRepository, IHttpContextAccessor httpContextAccessor, CourseContext dbContext
, GetService getService, ValidationService validationService)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _sellerRepository = sellerRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _pageRepository = pageRepository;
            _testRepository = testRepository;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _getService = getService;
            _validationService = validationService;
        }

        public async Task<TestResponse> CreateTest(CreateTestRequest request)
        {
            var validSeller = await _validationService.GetPageOwner(request.PageId);
            if (validSeller == false)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
            }
            var test = new Test
            {
                Title = request.Title,
                Description = request.Description,
                PageId = request.PageId,
            };
            await _testRepository.AddAsync(test);
            return new TestResponse(200, true, "Тест добавлен!", test.Id);
        }

        public async Task<TestDTOResponse> GetTestById(int testId)
        {
            var test = _dbContext.Tests.FirstOrDefault(p => p.Id == testId);
            if (test == null)
            {
                throw new Exception("Тест с таким айди не найден!");
            }
            var testDto = new TestDTO
            {
                Id = test.Id,
                Title = test.Title,
                Description = test.Description,
                PageId = test.PageId,
            };
            return new TestDTOResponse(200, true, null, testDto);
        }

        public async Task<TestResponse> UpdateTest(UpdateTestRequest request)
        {
            var test = await _testRepository.GetByIdAsync(request.TestId);
            if (test == null)
            {
                throw new Exception("Тест не найден.");
            }

            var validSeller = await _validationService.GetTestOwner(request.TestId);
            if (validSeller == false)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
            }

            test.Title = request.Title;
            test.Description = request.Description;
            await _testRepository.UpdateAsync(test);
            return new TestResponse(200, true, "Тест обновлен!", test.Id);
        }

        public async Task<Response> DeleteTest(int testId)
        {
            var test = await _testRepository.GetByIdAsync(testId);
            if (test == null)
            {
                throw new Exception("Тест с таким айди не найден");
            }
            var validSeller = await _validationService.GetTestOwner(testId);
            if (validSeller == false)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
            }

            await _testRepository.DeleteAsync(testId);
            return new Response(200, true, "Тест успешно удален!");
        }

        public async Task<List<TestDTO>> GetAllTestsByPageId(int pageId)
        {

            var tests = _dbContext.Tests.Where(p => p.PageId == pageId).ToList();
            var testsDto = new List<TestDTO>();

            for (int i = 0; i < tests.Count; i++)
            {
                var testDto = new TestDTO()

                {
                    Title = tests[i].Title,
                    PageId = tests[i].PageId,
                    Description = tests[i].Description,
                    Id = tests[i].Id,
                };
                testsDto.Add(testDto);
            }
            return testsDto;
        }
    }
}
