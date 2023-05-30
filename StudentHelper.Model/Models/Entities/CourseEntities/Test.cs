using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Entities.CourseEntities
{
    public class Test : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageId { get; set; }
        public Page? Page { get; set; }
        public List<Question> Questions { get; set; }
    }
}
