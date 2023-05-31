using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common
{
    public class PaymentForCourseResponse : Response
    {
        public PaymentForCourseResponse(int statusCode, bool success, string message, int courseId) : base(statusCode, success, message)
        {
            CourseId = courseId;
        }

        public int CourseId { get; set; }

    }
}
