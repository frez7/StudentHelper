using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class TestResponse : Response
    {
        public int TestId { get; set; }
        public TestResponse(int statusCode, bool success, string message, int testId) : base(statusCode, success, message)
        {
            TestId = testId;
        }

    }
}
