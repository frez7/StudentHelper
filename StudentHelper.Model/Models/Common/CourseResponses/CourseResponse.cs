using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class CourseResponse : Response
    {
        public int CourseId { get; set; }
        public CourseResponse(int statusCode, bool success, string message, int courseId) : base(statusCode, success, message)
        {
            CourseId = courseId;
        }
    }
}
