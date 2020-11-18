namespace Common
{
    public class LogConfiguration
    {
        public static LogConfiguration Current;

        public LogConfiguration()
        {
            Current = this;
        }

        public string MessageBrokerUrl { get; set; }
        public string LoggingCommandSubject { get; set; }
    }
}
