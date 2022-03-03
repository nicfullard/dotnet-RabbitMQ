using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ_Producer
{
    public static class HeaderExchangeProducer
    {
        public static string exchangeName => "demo-header-Exchange";
        public static string queueName => "demo-header-Queue";
        public static string routingKey => String.Empty;

        public static void Publish(IModel channel)
        {
            var args = new Dictionary<string, object>
            {
                { "x-message-ttl", 60000 },
            };

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Headers,
                arguments: args
            );

            var count = 0;
            do
            {
                var message = new
                {
                    Name = "Producer",
                    Sender = "Producer",
                    Timestamp = System.DateTime.Now,
                    Message = $"Message number {count}"
                };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object> { { "header_key", "header_value" } };

                Console.WriteLine("Publishing message:" + message.Message);
                channel.BasicPublish(exchangeName, routingKey, properties, body);

                count += 1;
                Thread.Sleep(1000);

            } while (count <= 100);
        }
    }
}
