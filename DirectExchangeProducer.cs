using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ_Producer
{
    public static class DirectExchangeProducer
    {
        public static string exchangeName => "demo-direct-Exchange";
        public static string queueName => "demo-direct-Queue";
        public static string routingKey => "demo-direct-key";

        public static void Publish(IModel channel)
        {
            var senderId = Guid.NewGuid().ToString();
            senderId = senderId.Substring(0, senderId.IndexOf('-'));

            var args = new Dictionary<string, object>
            {
                //{ "x-message-ttl", 60000 }
            };

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                arguments: args
            );

            var count = 0;
            do
            {
                var message = new
                {
                    Name = $"Message number {count}",
                    Sender = senderId,
                    Timestamp = DateTime.Now,
                    Message = $"Message Number {count} - {Guid.NewGuid()}"
                };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                Console.WriteLine($"{message.Timestamp} - Publisher: {message.Sender} - Message: { message.Message}");
                var props = channel.CreateBasicProperties();
                //props.Headers = new Dictionary<string, object>();
                //props.Headers.Add("OrderID", "FSA232eywkjhgdskj");

                channel.BasicPublish(exchangeName, routingKey, null, body);

                count += 1;
                Thread.Sleep(500);

            } while (count <= 10);
        }
    }
}
