using System.ComponentModel.DataAnnotations;

namespace StudentHelper.Model.Models.Requests.AdminRequests
{
    public class GetUserByNameRequest
    {
        [Required]
        public string UserName { get; set; }
    }
}
