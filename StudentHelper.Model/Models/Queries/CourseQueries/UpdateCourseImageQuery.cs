using MediatR;
using Microsoft.AspNetCore.Http;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.CourseQueries
{
    public class UpdateCourseImageQuery : IRequest<Response>
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }
}
