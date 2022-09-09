using System;
using System.Text;
using RabbitMQ.Client;

var Factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = Factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "myroutingexchange",
    type: ExchangeType.Direct
);

var message = "This message needs to be routed";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: "myroutingexchange", routingKey: "analyticsonly", null, body);

Console.WriteLine($"Published message: {message}");
