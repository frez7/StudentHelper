namespace StudentHelper.Model.Models.Entities.CourseDTOs
{
    public class EnrollmentDTO
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string StudentName { get; set; }
        public decimal ReceivedMoney { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
