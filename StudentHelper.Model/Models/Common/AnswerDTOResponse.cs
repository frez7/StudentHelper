using StudentHelper.Model.Models.Entities.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common
{
    public class AnswerDTOResponse : Response
    {
        public AnswerDTO? AnswerDTO { get; set; }
        public AnswerDTOResponse(int statusCode, bool success, string message, AnswerDTO answerDTO) : base(statusCode, success, message)
        {
            AnswerDTO = answerDTO;
        }

    }
}
