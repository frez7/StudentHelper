using MediatR;
using StudentHelper.Model.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Requests.UserRequests
{
    public class ForgotPasswordRequest : IRequest<Response>
    {
        [Required]
        public string RecipientEmail { get; set; }
    }
}
