using System.ComponentModel.DataAnnotations;

namespace StudentHelper.Model.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }

    }
}
