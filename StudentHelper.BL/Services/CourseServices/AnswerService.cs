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
using StudentHelper.Model.Models.Requests.CourseRequests.AnswerRequests;
using System.Security.Claims;

namespace StudentHelper.BL.Services.CourseServices
{
    public class AnswerService
    {
        private readonly IRepository<Answer> _answerRepository;
        private readonly CourseContext _dbContext;
        private readonly GetService _getService;
        private readonly ValidationService _validationService;

        public AnswerService(IRepository<Answer> answerRepository,
            CourseContext dbContext, ValidationService validationService, GetService getService)
        {
            _answerRepository = answerRepository;
            _dbContext = dbContext;
            _validationService = validationService;
            _getService = getService;
        }

        public async Task<AnswerResponse> CreateAnswer(CreateAnswerRequest request)
        {
            var id = _getService.GetCurrentUserId();
            var validSeller = await _validationService.GetQuestionOwner(request.QuestionId);
            if (validSeller == false)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
            }

            var answer = new Answer
            {
                Text = request.Text,
                IsCorrect = request.IsCorrect,
                QuestionId = request.QuestionId,
            };
            await _answerRepository.AddAsync(answer);
            return new AnswerResponse(200, true, "Ответ добавлен!", answer.Id);
        }


        public async Task<AnswerResponse> UpdateAnswer(UpdateAnswerRequest request)
        {
            var answer = await _answerRepository.GetByIdAsync(request.AnswerId);
            if (answer == null)
            {
                throw new Exception("Ответ не найден.");
            }

            var validSeller = await _validationService.GetAnswerOwner(request.AnswerId);
            if (validSeller == false)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
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
            var validSeller = await _validationService.GetAnswerOwner(answerId);
            if (validSeller == false)
            {
                throw new Exception("Вы не являетесь владельцем данного курса!");
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
