namespace StudentHelper.BL.Logging
{
    public class DbLoggerOptions
    {
        public string ConnectionString { get; set; }
        public string[] LogFields { get; set; }
        public string Logs { get; set; }
        public DbLoggerOptions() { }

    }
}
