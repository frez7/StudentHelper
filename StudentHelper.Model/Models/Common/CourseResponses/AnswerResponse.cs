using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class AnswerResponse : Response
    {
        public int AnswerId { get; set; }
        public AnswerResponse(int statusCode, bool success, string message, int answerId) : base(statusCode, success, message)
        {
            AnswerId = answerId;
        }
    }
}
