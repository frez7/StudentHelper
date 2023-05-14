using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common
{
    public class SellerApplicationResponse : Response
    {
        public SellerApplicationResponse(int statusCode, bool success, string message, string applicationStatus) : base(statusCode, success, message)
        {
            ApplicationStatus = applicationStatus;
        }
        public string ApplicationStatus { get; set; }
    }
}
