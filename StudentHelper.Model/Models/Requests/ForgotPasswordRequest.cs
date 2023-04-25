using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Requests
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string RecipientEmail { get; set; }

        //[Required]
        //public string Subject { get; set; }

        //[Required]
        //public string Body { get; set; }

    }
}
