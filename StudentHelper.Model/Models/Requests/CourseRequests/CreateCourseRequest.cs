
using StudentHelper.Model.Models.Entities.SellerEntities;
using System.ComponentModel.DataAnnotations;

namespace StudentHelper.Model.Models.Requests.CourseRequests
{
    public class CreateCourseRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFree { get; set; }
        public decimal Price { get; set; }
    }
}
