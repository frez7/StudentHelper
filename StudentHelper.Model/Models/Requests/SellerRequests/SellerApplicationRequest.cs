
using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Requests.SellerRequests
{
    public class SellerApplicationRequest : IRequest<Response>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string Email { get;set; }
    }
}
