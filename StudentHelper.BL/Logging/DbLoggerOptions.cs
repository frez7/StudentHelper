namespace StudentHelper.BL.Logging
{
    public class DbLoggerOptions
    {
        public string ConnectionString { get; set; }
        public string[] LogFields { get; set; }
        public string ErrorLogs { get; set; }
        public string InfoLogs { get; set; }
        public DbLoggerOptions() { }

    }
}
