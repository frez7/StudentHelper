using MediatR;
using StudentHelper.Model.Models.Common.CourseResponses;

namespace StudentHelper.Model.Models.Queries.PageQueries
{
    public class GetPageQuery : IRequest<PageDTOResponse>
    {
        public int PageId { get; set; }
    }
}
