using MediatR;
using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Queries.QuestionQueries
{
    public class GetAllQuestionsByTestIdQuery : IRequest<List<QuestionDTO>>
    {
        public int Id { get; set; }
    }
}
