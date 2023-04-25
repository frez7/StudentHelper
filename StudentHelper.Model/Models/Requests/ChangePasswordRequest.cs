
using System.ComponentModel.DataAnnotations;

namespace StudentHelper.Model.Models.Requests
{
    public class ChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}
