
namespace StudentHelper.Model.Models.Common
{
    public class UserResponse : Response
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public int StudentId { get; set; }
        public List<string> Roles { get; set; }
        public UserResponse(int statusCode, bool success, string message, string username, string email, int id, List<string> roles, int studentId) : base(statusCode, success, message)
        {
            UserName = username;
            Email = email;
            Id = id;
            Roles = roles;
            StudentId = studentId;
        }
    }
}
