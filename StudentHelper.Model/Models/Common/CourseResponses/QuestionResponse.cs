using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class QuestionResponse : Response
    {
        public int QuestionId { get; set; }
        public QuestionResponse(int statusCode, bool success, string message, int questionId) : base(statusCode, success, message)
        {
            QuestionId = questionId;
        }

    }
}
