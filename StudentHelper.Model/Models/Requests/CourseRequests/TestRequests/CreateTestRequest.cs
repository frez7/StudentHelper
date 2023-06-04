using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Requests.CourseRequests.TestRequests
{
    public class CreateTestRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageId { get; set; }


    }
}
