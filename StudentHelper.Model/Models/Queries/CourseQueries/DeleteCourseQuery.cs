using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.CourseQueries
{
    public class DeleteCourseQuery : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
