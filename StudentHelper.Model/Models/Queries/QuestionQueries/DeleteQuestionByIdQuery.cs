using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.QuestionQueries
{
    public class DeleteQuestionByIdQuery : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
