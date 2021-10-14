using Net6_Demo.Helpers;
using System.Collections.Concurrent;

namespace Net6_Demo.Providers
{
    public sealed class RabbitMQLoggerProvider : ILoggerProvider
    {
        private readonly RabbitMQHelper _rabbit;
        private readonly ConcurrentDictionary<string, RabbitMQLogger> _loggers = new();

        public RabbitMQLoggerProvider(RabbitMQHelper rabbit)
        {
            _rabbit = rabbit;
        }

        public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new RabbitMQLogger(name, _rabbit));

        public void Dispose()
        {
            _loggers.Clear();
        }
    }

    public class RabbitMQLogger : ILogger
    {
        private readonly string _name;
        private readonly RabbitMQHelper _rabbit;

        public RabbitMQLogger(string name, RabbitMQHelper rabbit) => (_name, _rabbit) = (name, rabbit);

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.None:
                case LogLevel.Debug:
                    return false;
                default:
                    return true;
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var log = new LogObject
            {
                LogLevel = logLevel,
                EventId = eventId.Id,
                Module = _name,
                LogData = formatter(state, exception)
            };
            if (exception != null)
            {
                log.ErrorCode = exception.HResult;
                log.ErrorMessage = exception.Message;
            }
            _rabbit.Publish(log);
        }
    }

    public class LogObject
    {
        public int EventId { get; set; }
        public LogLevel LogLevel { get; set; }
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string LogData { get; set; } = null!;
        public string Module { get; set; } = null!;
    }
    
}
