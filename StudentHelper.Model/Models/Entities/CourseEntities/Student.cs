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
        public List<StudentCourse>? Courses { get; set; }

    }
}
