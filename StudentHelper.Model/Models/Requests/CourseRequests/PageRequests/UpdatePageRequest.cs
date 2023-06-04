namespace StudentHelper.Model.Models.Requests.CourseRequests.PageRequests
{
    public class UpdatePageRequest
    {
        public int PageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }
}
