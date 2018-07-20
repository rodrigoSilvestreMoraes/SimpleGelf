using System.Collections.Generic;

namespace SimpleGelf.Extensions.Logging
{
    public class GelfLoggerConfig
    {
        public bool UsingAuthenticate { get; set; } = false;
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; } = 12201;
        public string LogSource { get; set; }
        public int HttpTimeoutSeconds { get; set; } = 30;
        public List<int> ListLogLevel { get; set; } = new List<int>();
    }
}