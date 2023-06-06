
using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.PageQueries
{
    public class DeletePageQuery : IRequest<Response>
    {
        public int PageId { get; set; }
    }
}
