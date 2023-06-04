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
using StudentHelper.Model.Models.Requests.CourseRequests.QuestionRequests;
using System.Security.Claims;

namespace StudentHelper.BL.Services.CourseServices
{
    public class QuestionService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Test> _testRepository;
        private readonly CourseContext _dbContext;
        private readonly GetService _getService;
        private readonly ValidationService _validationService;

        public QuestionService(CourseContext dbContext, IRepository<Test> testRepository
            , IRepository<Question> questionRepository
            , GetService getService, ValidationService validationService)
        {
            _dbContext = dbContext;
            _testRepository = testRepository;
            _questionRepository = questionRepository;
            _getService = getService;
            _validationService = validationService;
        }


        public async Task<QuestionResponse> CreateQuestion(CreateQuestionRequest request)
        {
            var id = _getService.GetCurrentUserId();
            var test = await _testRepository.GetByIdAsync(request.TestId);
            if (test == null)
            {
                return new QuestionResponse(400, false, "Тест с таким айди не найден!", 0);
            }
            var validSeller = await _validationService.GetTestOwner(request.TestId);
            if (validSeller == false)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
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

            var validSeller = await _validationService.GetQuestionOwner(request.QuestionId);
            if (validSeller == false)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
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
            var validSeller = await _validationService.GetQuestionOwner(questionId);
            if (validSeller == false)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
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
