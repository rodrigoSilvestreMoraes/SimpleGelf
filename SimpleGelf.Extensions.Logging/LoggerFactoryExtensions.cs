using Microsoft.Extensions.Logging;
using System;

namespace SimpleGelf.Extensions.Logging
{
    public static class LoggerFactoryExtensions
    {
        static GelfLoggerConfig _config;
        public static ILoggerFactory AddSimpleGelf(this ILoggerFactory loggerFactory, GelfLoggerConfig config)
        {
            _config = config;
            loggerFactory.AddProvider(new SimpleGelfProvider(config));
            GelfSimpleHttpClient.Configure(config);
            return loggerFactory;
        }

        public static void LogError(this ILogger logger, string message, object data, string guid, Exception exception = null)
        {
            if (_config == null)
                return;

            LogGelfLogErrorModel(guid, new LogErrorModel(message, exception, data));
        }

        private static void LogGelfLogErrorModel(string guid, LogErrorModel logErrorModel)
        {
            if (logErrorModel == null)
                return;

            var message = new GelfMessage
            {
                ShortMessage = logErrorModel.Message,
                Host = _config.LogSource,
                Level = SyslogSeverity.Error,
                Timestamp = GelfSimpleHttpClient.GetTimestamp(),
                Logger = "LoggerFactoryExtensions",
                Exception = logErrorModel._Exception?.ToString(),
                CorrelationId = guid,
                Data = logErrorModel.Data
            };

            GelfSimpleHttpClient.Send(message);
        }
    }
}
