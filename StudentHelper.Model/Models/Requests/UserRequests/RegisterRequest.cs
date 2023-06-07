using MediatR;
using StudentHelper.Model.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace StudentHelper.Model.Models.Requests.UserRequests
{
    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
