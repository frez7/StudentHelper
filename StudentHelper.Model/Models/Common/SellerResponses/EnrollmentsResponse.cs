using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Common.SellerResponses
{
    public class EnrollmentsResponse : Response
    {
        public List<EnrollmentDTO> Enrollments { get; set; }
        public decimal TotalMoney { get 
            {
                return Enrollments.Sum(m => m.ReceivedMoney);
            }
        }
        public EnrollmentsResponse(int statusCode, bool success, string message, List<EnrollmentDTO> enrollments) : base(statusCode, success, message)
        {
            Enrollments = enrollments;
        }
    }
}
