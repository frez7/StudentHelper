using MediatR;
using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Queries.CourseQueries
{
    public class SearchCoursesQuery : IRequest<List<CourseDTO>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Word { get; set; }
    }
}
