using MediatR;
using StudentHelper.Model.Models.Entities.CourseEntities;

namespace StudentHelper.Model.Models.Queries.CourseQueries
{
    public class GetAllStudentCoursesQuery : IRequest<List<Course>>
    {
        public int StudentId { get; set; }
    }
}
