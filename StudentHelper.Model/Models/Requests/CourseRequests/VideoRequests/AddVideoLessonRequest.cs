using MediatR;
using StudentHelper.Model.Models.Common.CourseResponses;

namespace StudentHelper.Model.Models.Requests.CourseRequests.VideoRequests
{
    public class AddVideoLessonRequest : IRequest<VideoResponse>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public int PageId { get; set; }
    }
}
