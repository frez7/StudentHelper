using MediatR;
using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Queries.CourseQueries
{
    public class GetCourseQuery : IRequest<CourseDTO>
    {
        public int Id { get; set; } 
    }
}