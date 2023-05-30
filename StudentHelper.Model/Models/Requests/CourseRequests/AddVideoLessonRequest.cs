namespace StudentHelper.Model.Models.Requests.CourseRequests
{
    public class AddVideoLessonRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public int PageId { get; set; }
    }
}
