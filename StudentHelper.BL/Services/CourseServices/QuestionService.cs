using Microsoft.AspNetCore.Http;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Data;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentHelper.Model.Models.Requests.CourseRequests.PageRequests;
using System.Security.Claims;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Requests.CourseRequests.AnswerRequests;
using StudentHelper.Model.Models.Requests.CourseRequests.QuestionRequests;

namespace StudentHelper.BL.Services.CourseServices
{
    public class QuestionService
    {
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Test> _testRepository;
        private readonly IRepository<Page> _pageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CourseContext _dbContext;

        public QuestionService(IRepository<Answer> answerRepository, IRepository<Seller> sellerRepository,
            IRepository<Course> courseRepository, IRepository<Student> studentRepository, IHttpContextAccessor httpContextAccessor,
            CourseContext dbContext, IRepository<Test> testRepository, IRepository<Page> pageRepository, IRepository<Question> questionRepository)
        {
            _answerRepository = answerRepository;
            _sellerRepository = sellerRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _testRepository = testRepository;
            _pageRepository = pageRepository;
            _questionRepository = questionRepository;
        }


        public async Task<QuestionResponse> CreateQuestion(CreateQuestionRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var seller = await _sellerRepository.GetByUserId(id);    
            var test = await _testRepository.GetByIdAsync(request.TestId);
            if (test == null)
            {
                return new QuestionResponse(400, false, "Тест с таким айди не найден!", 0);
            }
            if (seller == null)
            {
                return new QuestionResponse(400, false, "Ты не являешься продавцом!", 0);
            }
            var question = new Question
            {
                Text = request.Text,
                TestId = request.TestId,
            };
            await _questionRepository.AddAsync(question);
            return new QuestionResponse(200, true, "Вопрос добавлен!", question.Id);
        }

        public async Task<QuestionDTOResponse> GetQuestionById(int questionId)
        {
            var question = _dbContext.Questions.FirstOrDefault(p => p.Id == questionId);
            if (question == null)
            {
                throw new Exception("Вопрос с таким айди не найден!");
            }
            var questionDto = new QuestionDTO
            {
                Id = question.Id,
                Text = question.Text,
                TestId = question.TestId,
            };
            return new QuestionDTOResponse(200, true, null, questionDto);
        }

        public async Task<QuestionResponse> UpdateQuestion(UpdateQuestionRequest request)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId);
            if (question == null)
            {
                throw new Exception("Ответ не найден.");
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);

            var seller = await _sellerRepository.GetByUserId(id);
            var test = await _testRepository.GetByIdAsync(question.Id);
            var page = await _pageRepository.GetByIdAsync(test.PageId);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            if (seller.Id != course.SellerId)
            {
                throw new Exception("Вы не являетесь автором этого вопроса!");
            }

            question.Text = request.Text;
            await _questionRepository.UpdateAsync(question);
            return new QuestionResponse(200, true, "Вопрос обновлен!", question.Id);
        }

        public async Task<Response> DeleteQuestionById(int questionId)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                throw new Exception("Вопрос с таким айди не найден");
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var seller = await _sellerRepository.GetByUserId(id);
            var test = await _testRepository.GetByIdAsync(question.Id);
            var page = await _pageRepository.GetByIdAsync(test.PageId);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            if (seller.Id != course.SellerId)
            {
                throw new Exception("Вы не являетесь автором этого вопроса!");
            }

            await _questionRepository.DeleteAsync(questionId);
            return new Response(200, true, "Вопрос успешно удален!");
        }



        public async Task<List<QuestionDTO>> GetAllQuestionsByTestId(int testId)
        {

            var questions = _dbContext.Questions.Where(p => p.TestId == testId).ToList();
            var questionsDto = new List<QuestionDTO>();

            for (int i = 0; i < questions.Count; i++)
            {
                var questionDto = new QuestionDTO()

                {
                    Text = questions[i].Text,
                    TestId = questions[i].TestId,
                    Id = questions[i].Id,
                };
                questionsDto.Add(questionDto);
            }
            return questionsDto;
        }















    }
}
