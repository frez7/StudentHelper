namespace StudentHelper.Model.Models.Common.Other
{
    public class LogInfo : BaseEntity<int>
    {
        public string LogType { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
