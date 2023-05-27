using StudentHelper.Model.Models.Entities.CourseEntities;

namespace StudentHelper.Model.Models.Entities.CourseDTOs
{
    public class PageDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int CourseId { get; set; }
    }
}
