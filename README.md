# SimpleGelf
That class example demostrate as to use provider.
public class ApplicationLogging : ILoggingService
    {
        private static ILoggerFactory _Factory = null;
        const string providerConsole = "CONSOLE";
        const string providerFile = "FILE";
        const string providerGrayLog = "GRAYLOG";

        public ApplicationLogging(ILoggerFactory factory, LogConfig logConfig, GelfLoggerConfig gelfLoggerConfig)
        {
            var hasLoggerConsole = logConfig.Providers.Any(x => x.ToUpper().Equals(providerConsole));
            var hasLoggerFile = logConfig.Providers.Any(x => x.ToUpper().Equals(providerFile));
            var hasLoggerGrayLog = logConfig.Providers.Any(x => x.ToUpper().Equals(providerGrayLog));

            var configLogger = new LoggerConfiguration()
                   .MinimumLevel.Override("Microsoft", (LogEventLevel)logConfig.LogEventLevel)
                   .Enrich.FromLogContext();

            if (hasLoggerConsole)
                configLogger.WriteTo.Console();
            
            if(hasLoggerFile)
                configLogger.WriteTo.File($"{logConfig.NameFile}.txt",
                                            rollingInterval: RollingInterval.Day,
                                            flushToDiskInterval: new TimeSpan(logConfig.FlushToDiskIntervalHour, 0, 0),
                                            restrictedToMinimumLevel: (LogEventLevel)logConfig.LogEventLevel,
                                            rollOnFileSizeLimit: true);

            Log.Logger = configLogger.CreateLogger();

            if (hasLoggerGrayLog)
                factory.AddSimpleGelf(gelfLoggerConfig);

            _Factory = factory.AddSerilog(Log.Logger);
        }
     
        public ILoggerFactory LoggerFactory
        {
            get
            {
                return _Factory;
            }            
        }
    }
