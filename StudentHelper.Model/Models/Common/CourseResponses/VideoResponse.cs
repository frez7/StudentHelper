
namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class VideoResponse : Response
    {
        public int VideoId { get; set; }
        public VideoResponse(int statusCode, bool success, string message, int videoId) : base(statusCode, success, message)
        {
            VideoId = videoId;
        }
    }
}
