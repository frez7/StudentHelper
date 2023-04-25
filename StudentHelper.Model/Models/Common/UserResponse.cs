using StudentHelper.Model.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common
{
    public class UserResponse : Response
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public List<string> Roles { get; set; }
        public UserResponse(int statusCode, bool success, string message, string username, string email, string id, List<string> roles) : base(statusCode, success, message)
        {
            UserName = username;
            Email = email;
            Id = id;
            Roles = roles;
        }
    }
}
