using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.CourseQueries
{
    public class RemoveCourseFromStudentQuery : IRequest<Response>
    {
        public int CourseId { get; set; }
    }
}
