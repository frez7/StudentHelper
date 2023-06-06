
using MediatR;
using StudentHelper.Model.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace StudentHelper.Model.Models.Requests.UserRequests
{
    public class ChangePasswordRequest : IRequest<Response>
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}
