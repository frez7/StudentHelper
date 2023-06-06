using MediatR;
using StudentHelper.Model.Models.Common.CourseResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Requests.CourseRequests.TestRequests
{
    public class UpdateTestRequest : IRequest<TestResponse>
    {
        public int TestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
