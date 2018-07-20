using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimpleGelf.Extensions.Logging
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddSimpleGelf(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, SimpleGelfProvider>();
            return builder;
        }

        /// <summary>
        /// Registers a <see cref="GelfLoggerProvider"/> and <see cref="GelfLoggerOptions"/>
        /// with the service collection.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddSimpleGelf(this ILoggingBuilder builder, Action<GelfLoggerConfig> configure)
        {
            builder.AddSimpleGelf();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
