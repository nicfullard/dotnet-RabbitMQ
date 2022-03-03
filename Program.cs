// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ_Producer;


/*
const string username = "guest";
const string password = "guest";
const string protocol = "amqp";
const string host = "localhost";
const string port = "5672";
const string vhost = "";
*/


const string username = "ifgxxqrx";
const string password = "86IBE_C9l54F33PBzXyEsnTk6F77CDx2";
const string protocol = "amqps";
const string host = "cow.rmq2.cloudamqp.com";
const string port = "5671";
const string vhost = "ifgxxqrx";


Console.WriteLine($"Using server {host}");

var factory = new ConnectionFactory();
factory.Uri = new Uri($"{protocol}://{username}:{password}@{host}:{port}/{vhost}");

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//QueueProducer.Publish(channel);
//DirectExchangeProducer.Publish(channel);
//DirectExchangeProducer_File.Publish(channel);
//FanoutExchangeProducer.Publish(channel);
//PubSubExchangeProducer.Publish(channel);
//TopicExchangeProducer.Publish(channel);
//HeaderExchangeProducer.Publish(channel);

//ChatConsumer.Consume(channel);
//ChatProducer.Publish(channel);

vCommerceTest.Publish(channel);