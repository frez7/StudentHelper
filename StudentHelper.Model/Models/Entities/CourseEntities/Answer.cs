using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Entities.CourseEntities
{
    public class Answer : BaseEntity<int>
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
