using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class PageDTOResponse : Response
    {
        public PageDTO? PageDTO { get; set; }
        public PageDTOResponse(int statusCode, bool success, string message, PageDTO pageDTO) : base(statusCode, success, message)
        {
            PageDTO = pageDTO;
        }


    }
}
