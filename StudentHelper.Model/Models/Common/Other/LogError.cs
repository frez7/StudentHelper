namespace StudentHelper.Model.Models.Common.Other
{
    public class LogError : BaseEntity<int>
    {
        public string LogLevel { get; set; }
        public int ThreadId { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ExceptionSource { get;set; }
        public DateTime Created { get; set; }
    }
}
