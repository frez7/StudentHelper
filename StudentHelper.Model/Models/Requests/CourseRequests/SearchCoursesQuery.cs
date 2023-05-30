namespace StudentHelper.Model.Models.Requests.CourseRequests
{
    public class SearchCoursesQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Word { get; set; }
    }
}
