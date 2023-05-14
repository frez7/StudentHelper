
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.Model.Models.Entities.CourseEntities
{
    public class Course: BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFree { get; set; }
        public bool? IsPublished { get; set; }
        public decimal Price { get; set; }
        public Seller Seller { get; set; }
        public int SellerId { get; set; }
        public List<StudentCourse> Students { get; set; }
    }
}
