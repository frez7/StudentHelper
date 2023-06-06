using MediatR;
using StudentHelper.Model.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace StudentHelper.Model.Models.Requests.AdminRequests
{
    public class GetUserByNameRequest : IRequest<UserResponse>
    {
        [Required]
        public string UserName { get; set; }
    }
}
