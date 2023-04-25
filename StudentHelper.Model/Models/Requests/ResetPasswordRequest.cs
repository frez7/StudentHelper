using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Requests
{
    public class ResetPasswordRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
