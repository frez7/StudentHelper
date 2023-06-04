using StudentHelper.Model.Models.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Entities.SellerEntities
{
    public class SellerDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal? MoneyBalance { get; set; } = 0;
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
    }
}
