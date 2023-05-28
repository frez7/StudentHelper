using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class PageResponse : Response
    {
        public PageDTO? PageDTO { get; set; }
        public PageResponse(int statusCode, bool success, string message, PageDTO pageDTO) : base(statusCode, success, message)
        {
            PageDTO = pageDTO;
        }


    }
}
