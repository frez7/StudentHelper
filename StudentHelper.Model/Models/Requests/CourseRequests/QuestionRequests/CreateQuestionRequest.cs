using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Requests.CourseRequests.QuestionRequests
{
    public class CreateQuestionRequest
    {
        public string Text { get; set; }
        public int TestId { get; set; }
    }
}
