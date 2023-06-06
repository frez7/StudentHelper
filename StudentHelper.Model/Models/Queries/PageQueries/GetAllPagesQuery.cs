using MediatR;
using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Queries.PageQueries
{
    public class GetAllPagesQuery : IRequest<List<PageDTO>>
    {
        public int CourseId { get; set; }
    }
}
