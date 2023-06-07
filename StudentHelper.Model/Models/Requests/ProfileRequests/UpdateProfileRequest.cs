using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Requests.ProfileRequests
{
    public class UpdateProfileRequest
    {
        public string AboutMe { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
