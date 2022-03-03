﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ_Producer
{
    public static class TopicExchangeProducer
    {
        public static string exchangeName => "demo-topic-Exchange";
        public static string queueName => "demo-topic-Queue";
        public static string routingKey => "demo.topic.init";

        public static void Publish(IModel channel)
        {
            var args = new Dictionary<string, object>
            {
                { "x-message-ttl", 60000 }
            };

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Topic,
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

                //var properties = channel.CreateBasicProperties();

                Console.WriteLine("Publishing message:" + message.Message);
                channel.BasicPublish(exchangeName, routingKey, null, body);

                count += 1;
                Thread.Sleep(1000);

            } while (count <= 100);
        }
    }
}
