using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ_Producer
{
    public static class DirectExchangeProducer_File
    {
        public static string exchangeName => "demo-file-Exchange";
        public static string queueName => "demo-file-Queue";
        public static string routingKey => "demo-file-key";

        public static void Publish(IModel channel)
        {
            var args = new Dictionary<string, object>
            {
                { "x-message-ttl", 60000 }
            };

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                arguments: args
            );

            foreach (var file in Directory.GetFiles(@"C:\Temp\in"))
            {
                var fileInfo = new FileInfo(file);
                var fileData = File.ReadAllBytes(file);
                Console.WriteLine($"Publishing message: {fileInfo.Name}");

                var properties = channel.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object>();
                properties.Headers.Add("filename", fileInfo.Name);

                channel.BasicPublish(exchangeName, routingKey, properties, fileData);
            }
        }
    }
}
