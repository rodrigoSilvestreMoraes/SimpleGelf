# SimpleGelf
That class example demostrate as to use provider.

The appsettings.json file may config:

"LogConfig": {
"NameFile": "C:\Logs\ApiBIlling\apiLog",
"LogEventLevel": 4,
"FlushToDiskIntervalHour": 24,
"Providers": [ "File", "Console", "Graylog" ]
},
"GelfLoggerConfig": {
"User": "",
"Password": "",
"UsingAuthenticate": false,
"Host": "http://127.0.0.1",
"Port": 12201,
"LogSource": "billing_api",
"HttpTimeoutSeconds": 30,
"ListLogLevel": [ 4, 5 ]
},

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
