
using StudentHelper.Model.Enums;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Entities.SellerEntities
{
    public class SellerApplication : BaseEntity<int>
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public SellerApplicationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
