using StudentHelper.Model.Models.Entities.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class TestDTOResponse : Response
    {

        public TestDTO? TestDTO { get; set; }
        public TestDTOResponse(int statusCode, bool success, string message, TestDTO testDTO) : base(statusCode, success, message)
        {
            TestDTO = testDTO;
        }

    }
}
