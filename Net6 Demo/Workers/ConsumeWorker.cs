using Net6_Demo.Helpers;
using Net6_Demo.Providers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Net6_Demo.Workers
{
    public class ConsumeWorker : IWorker
    {
        private readonly IModel _channel;
        private readonly object _printLock = new();

        public ConsumeWorker(RabbitMQHelper mq)
        {
            _channel = mq.GetConnection();
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{GetType().Name} doing background work.");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                PrintLog(message);
                
            };
            _channel.BasicConsume(queue: "message",
                                 autoAck: true,
                                 consumer: consumer);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Lazy to setup a demo log viewer, so just write on console
        /// </summary>
        /// <param name="message"></param>
        private void PrintLog(string message)
        {
            try
            {
                var log = JsonSerializer.Deserialize<LogObject>(message);
                if(log == null)
                {
                    Console.WriteLine($"{DateTime.UtcNow} Received from RabbitMq: {message}");
                    return;
                }

                lock (_printLock)
                {
                    Console.WriteLine($"{DateTime.UtcNow:g} Received from RabbitMq:");
                    switch (log.LogLevel)
                    {
                        case LogLevel.Information:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case LogLevel.Warning:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case LogLevel.Error:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                    Console.Write(log.LogLevel);
                    Console.ResetColor();
                    Console.WriteLine($" [{log.EventId}] {log.Module}");
                    Console.WriteLine($" LogTxt: {log.LogData}");
                    if (!string.IsNullOrEmpty(log.ErrorMessage))
                    {
                        Console.WriteLine($" Error Code: {log.ErrorCode}, Message:{log.ErrorMessage}");
                    }
                }

            }
            catch (Exception)   //ignore
            {
                Console.WriteLine($"{DateTime.UtcNow} Received from RabbitMq: {message}");
            }
            
        }
    }
}
