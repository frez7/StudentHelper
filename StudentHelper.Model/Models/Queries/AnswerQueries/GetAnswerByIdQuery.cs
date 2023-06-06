using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.AnswerQueries
{
    public class GetAnswerByIdQuery : IRequest<AnswerDTOResponse>
    {
        public int Id { get; set; }
    }
}
