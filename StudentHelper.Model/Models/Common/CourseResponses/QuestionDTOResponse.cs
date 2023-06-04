using StudentHelper.Model.Models.Entities.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class QuestionDTOResponse : Response
    {
        public QuestionDTO? QuestionDTO { get; set; }
        public QuestionDTOResponse(int statusCode, bool success, string message, QuestionDTO questionDTO) : base(statusCode, success, message)
        {
            QuestionDTO = questionDTO;
        }
    }
}
