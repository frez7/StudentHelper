using MediatR;
using StudentHelper.Model.Models.Common.CourseResponses;

namespace StudentHelper.Model.Models.Queries.QuestionQueries
{
    public class GetQuestionByIdQuery : IRequest<QuestionDTOResponse>
    {
        public int Id { get; set; }
    }
}
