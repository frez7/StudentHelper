using MediatR;
using StudentHelper.Model.Models.Common.CourseResponses;

namespace StudentHelper.Model.Models.Queries.TestQueries
{
    public class GetTestByIdQuery : IRequest<TestDTOResponse>
    {
        public int TestId { get; set; }
    }
}
