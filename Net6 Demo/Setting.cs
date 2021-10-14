namespace Net6_Demo.Helpers
{
    public class RabbitMQSetting
    {
        public string Host { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string RoutingKey { get; set; } = null!;
        public string ExchangeId { get; set; } = null!;
        public int RabbitPort { get; set; }
    }
}
