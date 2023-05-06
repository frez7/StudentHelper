
namespace StudentHelper.Model.Models.Common
{
    public class AuthResponse : Response
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public AuthResponse(int statusCode, bool success, string message, string accessToken, string refreshToken, string userName) : base(statusCode, success, message)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            UserName = userName;
        }
    }
}
