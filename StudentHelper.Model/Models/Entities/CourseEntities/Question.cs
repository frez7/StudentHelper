using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Entities.CourseEntities
{
    public class Question : BaseEntity<int>
    {
        public string Text { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public List<Answer>? Answers { get; set; }= new List<Answer>();
    }
}
