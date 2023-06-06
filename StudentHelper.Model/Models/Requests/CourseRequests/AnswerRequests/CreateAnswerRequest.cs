using MediatR;
using StudentHelper.Model.Models.Common.CourseResponses;

namespace StudentHelper.Model.Models.Requests.CourseRequests.AnswerRequests
{
    public class CreateAnswerRequest : IRequest<AnswerResponse>
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

    }
}
