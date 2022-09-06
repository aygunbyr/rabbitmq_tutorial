using System;
using System.Text;
using RabbitMQ.Client;

var Factory = new ConnectionFactory{HostName = "localhost"};

using var connection = Factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

var message = "Hello I want to broadcast this message";

var encodedMessage = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: "pubsub", "", null, encodedMessage);

Console.WriteLine($"Published message: {message}");
