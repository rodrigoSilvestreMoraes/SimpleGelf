using Microsoft.Extensions.Logging;

namespace SimpleGelf.Extensions.Logging
{
    public class SimpleGelfProvider : ILoggerProvider
    {
        GelfLoggerConfig _config = null;
        public SimpleGelfProvider(GelfLoggerConfig config)
        {
            _config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new GelfSimpleLogger(categoryName, _config);
        }

        public void Dispose()
        {
            return;
        }
    }
}
