using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace StudentHelper.Model.Models.Requests
{
    public class EmailRequest
    {
        [Required]
        public string RecipientEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

    }
}
