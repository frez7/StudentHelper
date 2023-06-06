using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.TestQueries
{
    public class DeleteTestQuery : IRequest<Response>
    {
        public int TestId { get; set; }
    }
}
