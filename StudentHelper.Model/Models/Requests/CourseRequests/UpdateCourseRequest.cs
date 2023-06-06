

using MediatR;
using StudentHelper.Model.Models.Common.CourseResponses;

namespace StudentHelper.Model.Models.Requests.CourseRequests
{
    public class UpdateCourseRequest : IRequest<CourseResponse>
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFree { get; set; }
        public decimal Price { get; set; }
    }
}
