using StudentHelper.Model.Models.Common;
using static System.Net.Mime.MediaTypeNames;

namespace StudentHelper.Model.Models.Entities.CourseEntities
{
    public class Page : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public List<VideoLesson> VideoLessons { get; set; }
        public List<Test> Tests { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
