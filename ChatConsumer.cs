using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ_Producer
{
    public static class ChatConsumer
    {
        public static string exchangeName => "demo-chat-Exchange";
        public static string queueName => "demo-chat-Queue";
        public static string routingKey => "demo-chat-key";
        public static string senderId => "bevis";

        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct
            );

            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            channel.QueueBind(queueName, exchangeName, routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var obj = JsonConvert.DeserializeObject(message) as dynamic;
                if (obj != null)
                    if (obj.Sender != senderId)
                        Console.WriteLine($"{obj.Timestamp} - {obj.Sender}: {obj.Message}");

            };
            channel.BasicConsume(queueName, true, consumer);
        }
    }
}
