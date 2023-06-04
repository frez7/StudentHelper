using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class PageResponse : Response
    {

        public int PageId { get; set; }
        public PageResponse(int statusCode, bool success, string message, int pageId) : base(statusCode, success, message)
        {
            PageId = pageId;
        }


    }
}
