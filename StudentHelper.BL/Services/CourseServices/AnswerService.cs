using Microsoft.AspNetCore.Http;
using StudentHelper.Model.Data;
using StudentHelper.Model.Data.Repository;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Common.CourseResponses;
using StudentHelper.Model.Models.Entities.CourseDTOs;
using StudentHelper.Model.Models.Entities.CourseEntities;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Requests.CourseRequests.AnswerRequests;
using System.Security.Claims;

namespace StudentHelper.BL.Services.CourseServices
{
    public class AnswerService
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

        public AnswerService(IRepository<Answer> answerRepository, IRepository<Seller> sellerRepository,
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

        public async Task<AnswerResponse> CreateAnswer(CreateAnswerRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var seller = await _sellerRepository.GetByUserId(id);
            var question = await _questionRepository.GetByIdAsync(request.QuestionId);
            var test = await _testRepository.GetByIdAsync(question.TestId);
            var page = await _pageRepository.GetByIdAsync(test.PageId);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            if (seller.Id != course.SellerId)
            {
                return new AnswerResponse(400, false, "Вы не являетесь владельцем данного курса!", 0);
            }

            var answer = new Answer
            {
                Text = request.Text,
                IsCorrect = request.IsCorrect,
                QuestionId = request.QuestionId,
            };
            await _answerRepository.AddAsync(answer);
            await _questionRepository.UpdateAsync(question);
            return new AnswerResponse(200, true, "Ответ добавлен!", answer.Id);
        }


        public async Task<AnswerResponse> UpdateAnswer(UpdateAnswerRequest request)
        {
            var answer = await _answerRepository.GetByIdAsync(request.AnswerId);
            if (answer == null)
            {
                throw new Exception("Ответ не найден.");
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);

            var seller = await _sellerRepository.GetByUserId(id);
            var question = await _questionRepository.GetByIdAsync(answer.QuestionId);
            var test = await _testRepository.GetByIdAsync(question.Id);
            var page = await _pageRepository.GetByIdAsync(test.PageId);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);
            if (seller.Id != course.SellerId)
            {
                throw new Exception("Вы не являетесь автором этого вопроса!");
            }

            answer.Text = request.Text;
            answer.IsCorrect = request.IsCorrect;
            await _answerRepository.UpdateAsync(answer);
            return new AnswerResponse(200, true, "Вопрос обновлен!", answer.Id);
        }


        public async Task<AnswerDTOResponse> GetAnswerById(int answerId)
        {
            var answer = _dbContext.Answers.FirstOrDefault(p => p.Id == answerId);
            if (answer == null)
            {
                throw new Exception("Ответ с таким айди не найдена!");
            }
            var answerDto = new AnswerDTO
            {
                Id = answerId,
                Text = answer.Text,
                IsCorrect = answer.IsCorrect,
                QuestionId = answer.QuestionId,
            };
            return new AnswerDTOResponse(200, true, null, answerDto);
        }

        public async Task<Response> DeleteAnswerById(int answerId)
        {
            var answer = await _answerRepository.GetByIdAsync(answerId);
            if (answer == null)
            {
                throw new Exception("Ответ с таким айди не найден");
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out var id);
            var seller = await _sellerRepository.GetByUserId(id);
            var question = await _questionRepository.GetByIdAsync(answer.QuestionId);
            var test = await _testRepository.GetByIdAsync(question.TestId);
            var page = await _pageRepository.GetByIdAsync(test.PageId);
            var course = await _courseRepository.GetByIdAsync(page.CourseId);

            if (seller.Id != course.SellerId)
            {
                throw new Exception("Вы не являетесь владельцем данного вопроса!");
            }

            await _answerRepository.DeleteAsync(answerId);
            return new Response(200, true, "Ответ у вопроса успешно удален!");
        }

        public async Task<List<AnswerDTO>> GetAllAnswersByQuestionId(int questionId)
        {

            var answers = _dbContext.Answers.Where(p => p.QuestionId == questionId).ToList();
            var answersDto = new List<AnswerDTO>();

            for (int i = 0; i < answers.Count; i++)
            {
                var answerDto = new AnswerDTO()

                {
                    Text = answers[i].Text,
                    IsCorrect = answers[i].IsCorrect,
                    Id = answers[i].Id,
                    QuestionId = answers[i].QuestionId
                };
                answersDto.Add(answerDto);
            }
            return answersDto;
        }
    }
}
