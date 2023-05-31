using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Entities.SellerEntities
{
    public class Seller : BaseEntity<int>
    {
        public int UserId { get; set; }
        public bool IsConfirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal MoneyBalance { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public List<Course> Courses { get; set; }
         
    }
}
