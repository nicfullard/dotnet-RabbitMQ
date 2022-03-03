using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ_Producer
{
    public static class QueueProducer
    {
        public static string queueName => "demo-Queue";

        public static void Publish(IModel channel)
        {
            channel.QueueDeclare(
                queue: QueueProducer.queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
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

                Console.WriteLine("Publishing message:" + message.Message);
                channel.BasicPublish("", queueName, null, body);

                count += 1;
                //Thread.Sleep(1000);

            } while (count <= 1000000);
        }
    }
}
