namespace StudentHelper.Model.Models.Requests.CourseRequests.AnswerRequests
{
    public class CreateAnswerRequest
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

    }
}
