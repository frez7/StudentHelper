using MediatR;
using StudentHelper.Model.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace StudentHelper.Model.Models.Requests.UserRequests
{
    public class LoginRequest : IRequest<AuthResponse>
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
