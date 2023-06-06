using MediatR;

namespace StudentHelper.Model.Models.Queries.CourseQueries
{
    public class GetAllCourseStudentsQuery : IRequest<List<ApplicationUserDTO>>
    {
        public int CourseId { get; set; }
    }
}
