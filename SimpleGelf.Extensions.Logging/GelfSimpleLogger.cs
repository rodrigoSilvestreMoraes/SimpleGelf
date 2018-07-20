using Microsoft.Extensions.Logging;
using System;

namespace SimpleGelf.Extensions.Logging
{
    public class GelfSimpleLogger : ILogger
    {
        GelfLoggerConfig _gelfLoggerConfig;
        private readonly string _name;

        public GelfSimpleLogger(string name, GelfLoggerConfig gelfLoggerConfig)
        {
            _name = name;
            _gelfLoggerConfig = gelfLoggerConfig;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _gelfLoggerConfig.ListLogLevel.Contains((int)logLevel);
        }       

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = new GelfMessage
            {
                ShortMessage = formatter(state, exception),
                Host = _gelfLoggerConfig.LogSource,
                Level = GetLevel(logLevel),
                Timestamp = GelfSimpleHttpClient.GetTimestamp(),
                Logger = _name,
                Exception = exception?.ToString(),
                EventId = eventId.Id,
                EventName = eventId.Name                
            };
            GelfSimpleHttpClient.Send(message);
        }

      

        private static SyslogSeverity GetLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return SyslogSeverity.Debug;
                case LogLevel.Information:
                    return SyslogSeverity.Informational;
                case LogLevel.Warning:
                    return SyslogSeverity.Warning;
                case LogLevel.Error:
                    return SyslogSeverity.Error;
                case LogLevel.Critical:
                    return SyslogSeverity.Critical;
                default:
                    return SyslogSeverity.Informational;
            }
        }

       

      
    }
    public enum SyslogSeverity
    {
        Emergency = 0,
        Alert = 1,
        Critical = 2,
        Error = 3,
        Warning = 4,
        Notice = 5,
        Informational = 6,
        Debug = 7
    }
}
