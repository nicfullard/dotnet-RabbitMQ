using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ_Producer
{
    public static class ChatProducer
    {
        public static string exchangeName => "demo-chat-Exchange";
        public static string queueName => "demo-chat-Queue";
        public static string routingKey => "demo-chat-key";
        public static string senderId => "bevis";

        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct
            );

            var value = string.Empty;
            do
            {
                value = Console.ReadLine();
                if (string.IsNullOrEmpty(value))
                    value = "exit";

                var message = new
                {
                    Name = senderId,
                    Sender = senderId,
                    Timestamp = DateTime.Now,
                    Message = value
                };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(exchangeName, routingKey, null, body);
                Console.WriteLine($"{DateTime.Now} - {senderId} - message sent");
            } while (value.ToLower() != "exit");
        }
    }
}
