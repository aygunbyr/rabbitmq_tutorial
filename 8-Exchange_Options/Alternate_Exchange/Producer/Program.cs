using System;
using System.Text;
using RabbitMQ.Client;

var Factory = new ConnectionFactory(){HostName = "localhost"};

using var connection = Factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "altexchange", 
    type: ExchangeType.Fanout);

channel.ExchangeDeclare(
    exchange: "mainexchange", 
    type: ExchangeType.Direct,
    arguments: new Dictionary<String, Object>{
        {"alternate-exchange", "altexchange"}
    });

var message = "This is my first message";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("mainexchange", "test2", null, body);

Console.WriteLine($"Published message: {message}");
