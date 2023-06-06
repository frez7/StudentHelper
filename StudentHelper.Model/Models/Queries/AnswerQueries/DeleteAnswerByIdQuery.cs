using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.AnswerQueries
{
    public class DeleteAnswerByIdQuery : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
