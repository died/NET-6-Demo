using Microsoft.Extensions.DependencyInjection.Extensions;
using Net6_Demo.Providers;

namespace Net6_Demo.Extensions
{
    public static class ConfigExtension
    {
        public static ILoggingBuilder AddRabbitMQLogger(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, RabbitMQLoggerProvider>());
            return builder;
        }
    }
}
