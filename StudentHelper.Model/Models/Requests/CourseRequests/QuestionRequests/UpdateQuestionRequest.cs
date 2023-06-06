using MediatR;
using StudentHelper.Model.Models.Common.CourseResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Requests.CourseRequests.QuestionRequests
{
    public class UpdateQuestionRequest : IRequest<QuestionResponse>
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
    }
}
