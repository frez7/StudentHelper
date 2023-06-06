using MediatR;
using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Queries.AnswerQueries
{
    public class GetAllAnswersByQuestionIdQuery : IRequest<List<AnswerDTO>>
    {
        public int Id { get; set; }
    }
}
