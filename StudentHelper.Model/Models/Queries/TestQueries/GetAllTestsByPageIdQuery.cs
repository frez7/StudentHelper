using MediatR;
using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Queries.TestQueries
{
    public class GetAllTestsByPageIdQuery : IRequest<List<TestDTO>>
    {
        public int PageId { get; set; }
    }
}
