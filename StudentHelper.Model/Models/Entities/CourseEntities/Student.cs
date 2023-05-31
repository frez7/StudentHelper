using StudentHelper.Model.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Entities.CourseEntities
{
    public class Student : BaseEntity<int>
    {
        public int UserId { get; set; }
        public decimal? MoneyBalance { get; set; } = 0;
        public List<StudentCourse>? Courses { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AboutMe { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

    }
}
