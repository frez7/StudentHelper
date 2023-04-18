using System.Text.Json;

namespace StudentHelper.Model.Models.Common
{
    public class Response
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public Response(int statusCode, bool success, string message) 
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
