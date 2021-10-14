using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Net6_Demo.Helpers
{
    /// <summary>
    /// Handle RabbitMQ publish
    /// </summary>
    public class RabbitMQHelper
    {
        private readonly IModel _channel;
        private readonly string _routeKey;
        private readonly string _exchangeId;

        public RabbitMQHelper(IOptions<RabbitMQSetting> options)
        {
            var setting = options.Value;
            _routeKey = setting.RoutingKey;
            _exchangeId = setting.ExchangeId;
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = setting.Host,
                    UserName = setting.UserName,
                    Password = setting.Password,
                    Port = options.Value.RabbitPort,
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                //TODO
                Console.WriteLine(ex.Message);
            } 
        }

        /// <summary>
        /// Get connection for consumer
        /// </summary>
        /// <returns></returns>
        public IModel GetConnection()
        {
            return _channel;
        }

        /// <summary>
        /// Publish message
        /// </summary>
        /// <param name="message"></param>
        public void Publish(object message)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                _channel.BasicPublish(exchange: _exchangeId,
                                        routingKey: _routeKey,
                                        basicProperties: null,
                                        body: body);
            }
            catch (Exception ex)
            {
                //TODO
                Console.WriteLine(ex.Message);
            }
        }

        //just demo
        private Task Consume(string queue)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            _channel.BasicConsume(queue: queue,
                                 autoAck: true,
                                 consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
