using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Entities.SellerEntities
{
    public class Enrollment : BaseEntity<int>
    {
        public int SellerId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public decimal ReceivedMoney { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
