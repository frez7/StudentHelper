
using Microsoft.AspNetCore.Http;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Entities.SellerEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentHelper.Model.Models.Entities.CourseEntities
{
    public class Course: BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFree { get; set; }
        public bool? IsPublished { get; set; }
        public decimal Price { get; set; }
        public byte[]? Image { get; set; }

        [NotMapped]
        public string? ImageURL
        {
            get
            {
                if (Image != null && Image.Length > 0)
                {
                    return $"data:image/png;base64,{Convert.ToBase64String(Image)}";
                }
                else
                {
                    return null;
                }
            }
        }
        public Seller Seller { get; set; }
        public int SellerId { get; set; }
        public List<StudentCourse> Students { get; set; }
        public List<Page> Pages { get; set; }
    }
}
