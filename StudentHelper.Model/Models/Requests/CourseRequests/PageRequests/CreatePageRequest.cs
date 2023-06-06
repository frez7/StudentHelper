using MediatR;
using StudentHelper.Model.Models.Common.CourseResponses;

namespace StudentHelper.Model.Models.Requests.CourseRequests.PageRequests
{
    public class CreatePageRequest : IRequest<PageResponse>
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }
}
