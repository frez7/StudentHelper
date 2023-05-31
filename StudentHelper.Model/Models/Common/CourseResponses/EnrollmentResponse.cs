namespace StudentHelper.Model.Models.Common.CourseResponses
{
    public class EnrollmentResponse : Response
    {
        public int EnrollmentId { get; set; }
        public EnrollmentResponse(int statusCode, bool success, string message, int enrollmentId) : base(statusCode, success, message)
        {
            EnrollmentId = enrollmentId;
        }
    }
}
